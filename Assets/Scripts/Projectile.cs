using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

namespace BankShot
{
    [RequireComponent(typeof(BoxCollider2D))]
    [RequireComponent(typeof(Rigidbody2D))]
    [System.Serializable]
    public class Projectile : ProjectileBehavior
    {
        public float damage = 3;
        public float speed = 3f;
        public float life = 5f;
        public float bounceCooldown = 0.5f;
        public int maxBounces = 3;
        int bounces;
        Coroutine age;

        public Rigidbody2D rb;
        Vector2 velocity;

        bool bounced;
        byte owner;
        uint _lastLocalFrame;

        private void Awake()
        {
            rb = GetComponent<Rigidbody2D>();

            if (!NetworkManager.Instance.Networker.IsServer)
            {
                //rb.isKinematic = true;
            }
        }

        private void FixedUpdate()
        {
            if (NetworkManager.Instance.Networker.IsServer)
            {
                networkObject.position = rb.position;
            }
            else if(_lastLocalFrame <= networkObject.frame)
            {
                rb.velocity = Vector2.zero;
                rb.position = networkObject.position;
            }
        }

        public void Shoot(byte owner, Vector2 direction)
        {
            bounces = 0;
            this.owner = owner;

            Physics2D.IgnoreCollision(MatchManager.instance.players[owner].GetComponent<Collider2D>(),
                                      GetComponent<Collider2D>(), true);

            MainThreadManager.Run(() =>
            {
                networkObject.SendRpc(RPC_SHOOT, Receivers.Others,
                                        new object[] { owner, (Vector2)transform.position, direction });
            });

            gameObject.SetActive(true);

            age = StartCoroutine(Age());
            rb.velocity = speed * direction.normalized;
        }

        public void Die()
        {
            Physics2D.IgnoreCollision(MatchManager.instance.players[owner].GetComponent<Collider2D>(),
                                      GetComponent<Collider2D>(), false);
            StopCoroutine(age);
            gameObject.SetActive(false);
        }

        IEnumerator Age()
        {
            yield return new WaitForSeconds(life);

            MainThreadManager.Run(() =>
            {
                networkObject.SendRpc(RPC_DIE, Receivers.All);
            });
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (!NetworkManager.Instance.Networker.IsServer)
                return;

            if (collision.collider.tag == "Player")
            {
                Debug.Log("hit player");
                collision.collider.GetComponent<Player>().Damage(damage);
                MatchManager.instance.players[owner].Damage(-damage);

                MainThreadManager.Run(() =>
                {
                    networkObject.SendRpc(RPC_DIE, Receivers.All);
                });
            }
            else if (++bounces == maxBounces)
            {
                MainThreadManager.Run(() =>
                {
                    networkObject.SendRpc(RPC_DIE, Receivers.All);
                });
            }
        }

        public override void Die(RpcArgs args)
        {
            MainThreadManager.Run(() =>
            {
                Physics2D.IgnoreCollision(MatchManager.instance.players[owner].GetComponent<Collider2D>(),
                                      GetComponent<Collider2D>(), false);

                if (NetworkManager.Instance.Networker.IsServer)
                    StopCoroutine(age);

                networkObject.frame = 0;
                gameObject.SetActive(false);
            });
        }

        public override void Shoot(RpcArgs args)
        {
            MainThreadManager.Run(() =>
            {
                owner = args.GetNext<byte>();

                Physics2D.IgnoreCollision(MatchManager.instance.players[owner].GetComponent<Collider2D>(),
                                          GetComponent<Collider2D>(), true);

                _lastLocalFrame = 1;
                gameObject.SetActive(true);
                rb.position = args.GetNext<Vector2>();
                rb.velocity = speed * args.GetNext<Vector2>().normalized;
            });
        }
    }
}