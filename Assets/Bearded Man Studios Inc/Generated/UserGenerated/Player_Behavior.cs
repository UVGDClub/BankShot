using BeardedManStudios.Forge.Networking;
using BeardedManStudios.Forge.Networking.Unity;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedRPC("{\"types\":[[\"Vector2\"][][][\"float\"][\"Vector2\", \"Quaternion\"]]")]
	[GeneratedRPCVariableNames("{\"types\":[[\"dir\"][][][\"dmg\"][\"position\", \"rotation\"]]")]
	public abstract partial class Player_Behavior : NetworkBehavior
	{
		public const byte RPC_MOVE = 0 + 5;
		public const byte RPC_SHOOT = 1 + 5;
		public const byte RPC_INIT = 2 + 5;
		public const byte RPC_DAMAGE = 3 + 5;
		public const byte RPC_SPAWN = 4 + 5;
		
		public Player_NetworkObject networkObject = null;

		public override void Initialize(NetworkObject obj)
		{
			// We have already initialized this object
			if (networkObject != null && networkObject.AttachedBehavior != null)
				return;
			
			networkObject = (Player_NetworkObject)obj;
			networkObject.AttachedBehavior = this;

			base.SetupHelperRpcs(networkObject);
			networkObject.RegisterRpc("Move", Move, typeof(Vector2));
			networkObject.RegisterRpc("Shoot", Shoot);
			networkObject.RegisterRpc("Init", Init);
			networkObject.RegisterRpc("Damage", Damage, typeof(float));
			networkObject.RegisterRpc("Spawn", Spawn, typeof(Vector2), typeof(Quaternion));

			networkObject.onDestroy += DestroyGameObject;

			if (!obj.IsOwner)
			{
				if (!skipAttachIds.ContainsKey(obj.NetworkId)){
					uint newId = obj.NetworkId + 1;
					ProcessOthers(gameObject.transform, ref newId);
				}
				else
					skipAttachIds.Remove(obj.NetworkId);
			}

			if (obj.Metadata != null)
			{
				byte transformFlags = obj.Metadata[0];

				if (transformFlags != 0)
				{
					BMSByte metadataTransform = new BMSByte();
					metadataTransform.Clone(obj.Metadata);
					metadataTransform.MoveStartIndex(1);

					if ((transformFlags & 0x01) != 0 && (transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() =>
						{
							transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform);
							transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform);
						});
					}
					else if ((transformFlags & 0x01) != 0)
					{
						MainThreadManager.Run(() => { transform.position = ObjectMapper.Instance.Map<Vector3>(metadataTransform); });
					}
					else if ((transformFlags & 0x02) != 0)
					{
						MainThreadManager.Run(() => { transform.rotation = ObjectMapper.Instance.Map<Quaternion>(metadataTransform); });
					}
				}
			}

			MainThreadManager.Run(() =>
			{
				NetworkStart();
				networkObject.Networker.FlushCreateActions(networkObject);
			});
		}

		protected override void CompleteRegistration()
		{
			base.CompleteRegistration();
			networkObject.ReleaseCreateBuffer();
		}

		public override void Initialize(NetWorker networker, byte[] metadata = null)
		{
			Initialize(new Player_NetworkObject(networker, createCode: TempAttachCode, metadata: metadata));
		}

		private void DestroyGameObject(NetWorker sender)
		{
			MainThreadManager.Run(() => { try { Destroy(gameObject); } catch { } });
			networkObject.onDestroy -= DestroyGameObject;
		}

		public override NetworkObject CreateNetworkObject(NetWorker networker, int createCode, byte[] metadata = null)
		{
			return new Player_NetworkObject(networker, this, createCode, metadata);
		}

		protected override void InitializedTransform()
		{
			networkObject.SnapInterpolations();
		}

		/// <summary>
		/// Arguments:
		/// Vector2 dir
		/// </summary>
		public abstract void Move(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void Shoot(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void Init(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void Damage(RpcArgs args);
		/// <summary>
		/// Arguments:
		/// </summary>
		public abstract void Spawn(RpcArgs args);

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}