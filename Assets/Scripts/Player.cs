using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

namespace BankShot
{
    /*TODO: Integrate input frames for client behaviour to use a predictive client with
     * and authoritative server
     */
    public class Player : Player_Behavior
    {
        [Header("Projectile")]
        public Projectile projectilePrefab;
        public float projectileOffset = 0.5f;
        public float curCharge = 1;
        public float maxCharge = 3;
        public float deltaCharge = 0.1f;

        [Header("Vitals")]
        public float hitPoints = 100;
        public float maxHitPoints = 100;
        Coroutine healthDrain;

        [Header("Movement")]
        public Rigidbody2D rb;
        public float moveSpeed = 3f;
        Coroutine update;

        Vector2 move;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();
        }

        public void Initialize()
        {
            if (NetworkManager.Instance.Networker.IsServer)
                healthDrain = StartCoroutine(HealthDrain());

            if(networkObject.IsOwner)
                update = StartCoroutine(InputUpdate());

            Debug.Log(name + " is server: " + networkObject.IsServer
                           + "; is owner: " + networkObject.IsOwner
                           + "; network id: " + networkObject.NetworkId
                           + "; id: " + networkObject.id);
        }

        private void Update()
        {
            if (NetworkManager.Instance.Networker.IsServer)
            {
                networkObject.hp = hitPoints;
            }
            else
            {
                hitPoints = networkObject.hp;
            }

            if(networkObject.IsOwner)
            {
                networkObject.position = rb.position;
            }
            else
            {
                rb.position = networkObject.position;
                transform.rotation = networkObject.rotation;
            }
        }

        IEnumerator InputUpdate()
        {
            for (; ; )
            {
                move = Vector2.zero;
                if (Input.GetKey(KeyCode.A))
                {
                    move = Vector2.left;
                }
                else if (Input.GetKey(KeyCode.D))
                {
                    move = Vector2.right;
                }

                if (Input.GetKey(KeyCode.W))
                {
                    move += Vector2.up;
                }
                else if (Input.GetKey(KeyCode.S))
                {
                    move += Vector2.down;
                }

                move = move.normalized * moveSpeed;

                transform.right = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) 
                                  - (Vector2)transform.position;

                networkObject.rotation = transform.rotation;

                if (Input.GetMouseButton(0))
                {
                    curCharge += deltaCharge;
                }
                else if (Input.GetMouseButtonUp(0))
                {
                    MainThreadManager.Run(() =>
                    {
                        networkObject.SendRpc(RPC_SHOOT, Receivers.Server);
                    });
                }

                networkObject.SendRpc(RPC_MOVE, Receivers.All, move);

                yield return null;
            }
        }

        IEnumerator HealthDrain()
        {
            for (; ; )
            {
                if (hitPoints > 1)
                    hitPoints--;

                yield return new WaitForSeconds(1);
            }
        }

        public void Damage(float damage)
        {
            networkObject.SendRpc(RPC_DAMAGE, Receivers.All, damage);
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.magenta;
            Debug.DrawLine(rb.transform.position, rb.transform.position + transform.right.normalized);
        }

        public override void Move(RpcArgs args)
        {
            MainThreadManager.Run(() =>
            {
                rb.velocity = args.GetNext<Vector2>();
            });
        }

        public override void Shoot(RpcArgs args)
        {
            MainThreadManager.Run(() =>
            {
                Debug.Log("Shooting from player with id: " + networkObject.id);

                if (hitPoints > 1)
                    hitPoints--;

                Projectile p = ProjectilePool.instance.Get();

                p.transform.position = (Vector2)transform.position
                                        + (Vector2)transform.right * projectileOffset;

                p.networkObject.position = p.transform.position;

                p.damage *= curCharge;
                curCharge = 1;

                p.Shoot(networkObject.id, transform.right);
            });
        }

        public override void Init(RpcArgs args)
        {
            if (networkObject == null || !networkObject.IsOwner)
            {
                return;
            }

            MainThreadManager.Run(() =>
            {
                Initialize();
            });
        }

        public override void Damage(RpcArgs args)
        {
            MainThreadManager.Run(() =>
            {
                float damage = args.GetNext<float>();

                hitPoints -= damage;

                if (hitPoints <= 0)
                {
                    Debug.Log(name + " is dead");
                    //gameObject.SetActive(false);
                }
                else if (hitPoints > maxHitPoints)
                    hitPoints = maxHitPoints;
            });
        }

        public void Spawn(SpawnPoint sp)
        {
            MainThreadManager.Run(() =>
            {
                networkObject.position = sp.transform.position;
                rb.position = networkObject.position;
                networkObject.rotation = sp.transform.rotation;
                transform.rotation = networkObject.rotation;
                Initialize();
            });

            networkObject.SendRpc(RPC_SPAWN, Receivers.All, 
                                    (Vector2)sp.transform.position, 
                                    sp.transform.rotation);
        }

        public override void Spawn(RpcArgs args)
        {
            Debug.Log("Spawn rpc");

            MainThreadManager.Run(() =>
            {
                networkObject.position = args.GetNext<Vector2>();
                rb.position = networkObject.position;
                networkObject.rotation = args.GetNext<Quaternion>();
                transform.rotation = networkObject.rotation;
                Initialize();
            });
        }
    }
}