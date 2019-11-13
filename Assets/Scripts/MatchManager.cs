using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;
using BeardedManStudios.Forge.Networking;

namespace BankShot
{
    public class MatchManager : MatchManagerBehavior
    {
        public static MatchManager instance;

        public List<Player> players = new List<Player>();
        public List<SpawnPoint> spawnPoints = new List<SpawnPoint>();
        public int playerCount = 2;

        private void Awake()
        {
            if(instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        private void Start()
        {
            Debug.Log("IsServer: " + NetworkManager.Instance.IsServer);

            if (NetworkManager.Instance.IsServer)
            {
                StartCoroutine(Setup());
                Player_Behavior pb = NetworkManager.Instance.InstantiatePlayer_();
                ((Player)pb).Spawn(spawnPoints[players.Count]);
                players.Add((Player)pb);
                pb.networkObject.id = 0;
                Debug.Log(pb.networkObject.NetworkId);

                NetworkManager.Instance.Networker.playerAccepted += PlayerAccepted;
            }
        }

        private void PlayerAccepted(NetworkingPlayer player, NetWorker sender)
        {
            MainThreadManager.Run(() => {
                Player_Behavior pb = NetworkManager.Instance.InstantiatePlayer_();
                ((Player)pb).Spawn(spawnPoints[players.Count]);
                pb.networkObject.id = (byte)players.Count;
                players.Add((Player)pb);
                pb.networkObject.AssignOwnership(player);
            });
        }

        IEnumerator Setup()
        {
            while (players.Count < playerCount)
                yield return null;

            yield return new WaitForSeconds(1f);

            StartCoroutine(ProjectilePool.instance.Init());

            MainThreadManager.Run(() =>
            {
                networkObject.SendRpc(RPC_BUILD_PLAYER_LIST, Receivers.Others);
            });
        }

        public override void BuildPlayerList(RpcArgs args)
        {
            List<Player> plist = FindObjectsOfType<Player>().OfType<Player>().ToList();

            while(plist.Count > 0)
            {
                byte minID = plist[0].networkObject.id;
                int index = 0;
                for(int i = 0; i < plist.Count; i++)
                {
                    if (plist[i].networkObject.NetworkId < minID)
                    {
                        minID = plist[i].networkObject.id;
                        index = i;
                    }
                }

                players.Add(plist[index]);
                plist.RemoveAt(index);
            }
        }
    }
}
