using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking.Unity;

namespace BankShot
{
    [System.Serializable]
    public class ProjectilePool : ProjectilePoolBehavior
    {
        public static ProjectilePool instance;

        int index = 0;
        public int size;
        public List<Projectile> pool;

        private void Start()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public override void Init(RpcArgs args)
        {
            MainThreadManager.Run(() =>
            {
                List<Projectile> pjs = FindObjectsOfType<Projectile>().OfType<Projectile>().ToList();

                Debug.Log("Projectiles found: " + pjs.Count);

                while (pjs.Count > 0)
                {
                    uint minID = pjs[0].networkObject.NetworkId;
                    int index = 0;
                    for (int i = 0; i < pjs.Count; i++)
                    {
                        if (pjs[i].networkObject.NetworkId < minID)
                        {
                            minID = pjs[i].networkObject.NetworkId;
                            index = i;
                        }
                    }

                    pool.Add(pjs[index]);
                    pjs[index].transform.SetParent(transform);
                    pjs[index].gameObject.SetActive(false);
                    pjs[index].GetComponent<BoxCollider2D>().enabled = true;
                    pjs.RemoveAt(index);
                }
            });

            Debug.Log("pool initialized");
        }

        public IEnumerator Init()
        {
            if (NetworkManager.Instance.IsServer)
            {
                MainThreadManager.Run(() =>
                {
                    for (int i = 0; i < size; i++)
                    {
                        ProjectileBehavior p = NetworkManager.Instance.InstantiateProjectile();
                        pool.Add((Projectile)p);
                        ((Projectile)p).transform.SetParent(transform);
                        ((Projectile)p).gameObject.SetActive(false);
                        ((Projectile)p).GetComponent<BoxCollider2D>().enabled = true;
                    }

                });

                for(int i = 0; i < pool.Count; i++)
                {
                    while (!pool[i].networkObject.NetworkReady)
                        yield return null;
                }

                MainThreadManager.Run(() =>
                {
                    networkObject.SendRpc(RPC_INIT, Receivers.Others);
                });
            }
        }

        public Projectile Get()
        {
            while(pool[index].gameObject.activeSelf)
                index = (index + 1) % pool.Count;

            return pool[index];
        }
    }
}