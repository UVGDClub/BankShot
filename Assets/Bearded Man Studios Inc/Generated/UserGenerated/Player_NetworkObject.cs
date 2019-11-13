using BeardedManStudios.Forge.Networking.Frame;
using BeardedManStudios.Forge.Networking.Unity;
using System;
using UnityEngine;

namespace BeardedManStudios.Forge.Networking.Generated
{
	[GeneratedInterpol("{\"inter\":[0.15,0.15,0,0,0]")]
	public partial class Player_NetworkObject : NetworkObject
	{
		public const int IDENTITY = 7;

		private byte[] _dirtyFields = new byte[1];

		#pragma warning disable 0067
		public event FieldChangedEvent fieldAltered;
		#pragma warning restore 0067
		[ForgeGeneratedField]
		private Vector3 _position;
		public event FieldEvent<Vector3> positionChanged;
		public InterpolateVector3 positionInterpolation = new InterpolateVector3() { LerpT = 0.15f, Enabled = true };
		public Vector3 position
		{
			get { return _position; }
			set
			{
				// Don't do anything if the value is the same
				if (_position == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x1;
				_position = value;
				hasDirtyFields = true;
			}
		}

		public void SetpositionDirty()
		{
			_dirtyFields[0] |= 0x1;
			hasDirtyFields = true;
		}

		private void RunChange_position(ulong timestep)
		{
			if (positionChanged != null) positionChanged(_position, timestep);
			if (fieldAltered != null) fieldAltered("position", _position, timestep);
		}
		[ForgeGeneratedField]
		private Quaternion _rotation;
		public event FieldEvent<Quaternion> rotationChanged;
		public InterpolateQuaternion rotationInterpolation = new InterpolateQuaternion() { LerpT = 0.15f, Enabled = true };
		public Quaternion rotation
		{
			get { return _rotation; }
			set
			{
				// Don't do anything if the value is the same
				if (_rotation == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x2;
				_rotation = value;
				hasDirtyFields = true;
			}
		}

		public void SetrotationDirty()
		{
			_dirtyFields[0] |= 0x2;
			hasDirtyFields = true;
		}

		private void RunChange_rotation(ulong timestep)
		{
			if (rotationChanged != null) rotationChanged(_rotation, timestep);
			if (fieldAltered != null) fieldAltered("rotation", _rotation, timestep);
		}
		[ForgeGeneratedField]
		private float _charge;
		public event FieldEvent<float> chargeChanged;
		public InterpolateFloat chargeInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float charge
		{
			get { return _charge; }
			set
			{
				// Don't do anything if the value is the same
				if (_charge == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x4;
				_charge = value;
				hasDirtyFields = true;
			}
		}

		public void SetchargeDirty()
		{
			_dirtyFields[0] |= 0x4;
			hasDirtyFields = true;
		}

		private void RunChange_charge(ulong timestep)
		{
			if (chargeChanged != null) chargeChanged(_charge, timestep);
			if (fieldAltered != null) fieldAltered("charge", _charge, timestep);
		}
		[ForgeGeneratedField]
		private float _hp;
		public event FieldEvent<float> hpChanged;
		public InterpolateFloat hpInterpolation = new InterpolateFloat() { LerpT = 0f, Enabled = false };
		public float hp
		{
			get { return _hp; }
			set
			{
				// Don't do anything if the value is the same
				if (_hp == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x8;
				_hp = value;
				hasDirtyFields = true;
			}
		}

		public void SethpDirty()
		{
			_dirtyFields[0] |= 0x8;
			hasDirtyFields = true;
		}

		private void RunChange_hp(ulong timestep)
		{
			if (hpChanged != null) hpChanged(_hp, timestep);
			if (fieldAltered != null) fieldAltered("hp", _hp, timestep);
		}
		[ForgeGeneratedField]
		private byte _id;
		public event FieldEvent<byte> idChanged;
		public Interpolated<byte> idInterpolation = new Interpolated<byte>() { LerpT = 0f, Enabled = false };
		public byte id
		{
			get { return _id; }
			set
			{
				// Don't do anything if the value is the same
				if (_id == value)
					return;

				// Mark the field as dirty for the network to transmit
				_dirtyFields[0] |= 0x10;
				_id = value;
				hasDirtyFields = true;
			}
		}

		public void SetidDirty()
		{
			_dirtyFields[0] |= 0x10;
			hasDirtyFields = true;
		}

		private void RunChange_id(ulong timestep)
		{
			if (idChanged != null) idChanged(_id, timestep);
			if (fieldAltered != null) fieldAltered("id", _id, timestep);
		}

		protected override void OwnershipChanged()
		{
			base.OwnershipChanged();
			SnapInterpolations();
		}
		
		public void SnapInterpolations()
		{
			positionInterpolation.current = positionInterpolation.target;
			rotationInterpolation.current = rotationInterpolation.target;
			chargeInterpolation.current = chargeInterpolation.target;
			hpInterpolation.current = hpInterpolation.target;
			idInterpolation.current = idInterpolation.target;
		}

		public override int UniqueIdentity { get { return IDENTITY; } }

		protected override BMSByte WritePayload(BMSByte data)
		{
			UnityObjectMapper.Instance.MapBytes(data, _position);
			UnityObjectMapper.Instance.MapBytes(data, _rotation);
			UnityObjectMapper.Instance.MapBytes(data, _charge);
			UnityObjectMapper.Instance.MapBytes(data, _hp);
			UnityObjectMapper.Instance.MapBytes(data, _id);

			return data;
		}

		protected override void ReadPayload(BMSByte payload, ulong timestep)
		{
			_position = UnityObjectMapper.Instance.Map<Vector3>(payload);
			positionInterpolation.current = _position;
			positionInterpolation.target = _position;
			RunChange_position(timestep);
			_rotation = UnityObjectMapper.Instance.Map<Quaternion>(payload);
			rotationInterpolation.current = _rotation;
			rotationInterpolation.target = _rotation;
			RunChange_rotation(timestep);
			_charge = UnityObjectMapper.Instance.Map<float>(payload);
			chargeInterpolation.current = _charge;
			chargeInterpolation.target = _charge;
			RunChange_charge(timestep);
			_hp = UnityObjectMapper.Instance.Map<float>(payload);
			hpInterpolation.current = _hp;
			hpInterpolation.target = _hp;
			RunChange_hp(timestep);
			_id = UnityObjectMapper.Instance.Map<byte>(payload);
			idInterpolation.current = _id;
			idInterpolation.target = _id;
			RunChange_id(timestep);
		}

		protected override BMSByte SerializeDirtyFields()
		{
			dirtyFieldsData.Clear();
			dirtyFieldsData.Append(_dirtyFields);

			if ((0x1 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _position);
			if ((0x2 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _rotation);
			if ((0x4 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _charge);
			if ((0x8 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _hp);
			if ((0x10 & _dirtyFields[0]) != 0)
				UnityObjectMapper.Instance.MapBytes(dirtyFieldsData, _id);

			// Reset all the dirty fields
			for (int i = 0; i < _dirtyFields.Length; i++)
				_dirtyFields[i] = 0;

			return dirtyFieldsData;
		}

		protected override void ReadDirtyFields(BMSByte data, ulong timestep)
		{
			if (readDirtyFlags == null)
				Initialize();

			Buffer.BlockCopy(data.byteArr, data.StartIndex(), readDirtyFlags, 0, readDirtyFlags.Length);
			data.MoveStartIndex(readDirtyFlags.Length);

			if ((0x1 & readDirtyFlags[0]) != 0)
			{
				if (positionInterpolation.Enabled)
				{
					positionInterpolation.target = UnityObjectMapper.Instance.Map<Vector3>(data);
					positionInterpolation.Timestep = timestep;
				}
				else
				{
					_position = UnityObjectMapper.Instance.Map<Vector3>(data);
					RunChange_position(timestep);
				}
			}
			if ((0x2 & readDirtyFlags[0]) != 0)
			{
				if (rotationInterpolation.Enabled)
				{
					rotationInterpolation.target = UnityObjectMapper.Instance.Map<Quaternion>(data);
					rotationInterpolation.Timestep = timestep;
				}
				else
				{
					_rotation = UnityObjectMapper.Instance.Map<Quaternion>(data);
					RunChange_rotation(timestep);
				}
			}
			if ((0x4 & readDirtyFlags[0]) != 0)
			{
				if (chargeInterpolation.Enabled)
				{
					chargeInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					chargeInterpolation.Timestep = timestep;
				}
				else
				{
					_charge = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_charge(timestep);
				}
			}
			if ((0x8 & readDirtyFlags[0]) != 0)
			{
				if (hpInterpolation.Enabled)
				{
					hpInterpolation.target = UnityObjectMapper.Instance.Map<float>(data);
					hpInterpolation.Timestep = timestep;
				}
				else
				{
					_hp = UnityObjectMapper.Instance.Map<float>(data);
					RunChange_hp(timestep);
				}
			}
			if ((0x10 & readDirtyFlags[0]) != 0)
			{
				if (idInterpolation.Enabled)
				{
					idInterpolation.target = UnityObjectMapper.Instance.Map<byte>(data);
					idInterpolation.Timestep = timestep;
				}
				else
				{
					_id = UnityObjectMapper.Instance.Map<byte>(data);
					RunChange_id(timestep);
				}
			}
		}

		public override void InterpolateUpdate()
		{
			if (IsOwner)
				return;

			if (positionInterpolation.Enabled && !positionInterpolation.current.UnityNear(positionInterpolation.target, 0.0015f))
			{
				_position = (Vector3)positionInterpolation.Interpolate();
				//RunChange_position(positionInterpolation.Timestep);
			}
			if (rotationInterpolation.Enabled && !rotationInterpolation.current.UnityNear(rotationInterpolation.target, 0.0015f))
			{
				_rotation = (Quaternion)rotationInterpolation.Interpolate();
				//RunChange_rotation(rotationInterpolation.Timestep);
			}
			if (chargeInterpolation.Enabled && !chargeInterpolation.current.UnityNear(chargeInterpolation.target, 0.0015f))
			{
				_charge = (float)chargeInterpolation.Interpolate();
				//RunChange_charge(chargeInterpolation.Timestep);
			}
			if (hpInterpolation.Enabled && !hpInterpolation.current.UnityNear(hpInterpolation.target, 0.0015f))
			{
				_hp = (float)hpInterpolation.Interpolate();
				//RunChange_hp(hpInterpolation.Timestep);
			}
			if (idInterpolation.Enabled && !idInterpolation.current.UnityNear(idInterpolation.target, 0.0015f))
			{
				_id = (byte)idInterpolation.Interpolate();
				//RunChange_id(idInterpolation.Timestep);
			}
		}

		private void Initialize()
		{
			if (readDirtyFlags == null)
				readDirtyFlags = new byte[1];

		}

		public Player_NetworkObject() : base() { Initialize(); }
		public Player_NetworkObject(NetWorker networker, INetworkBehavior networkBehavior = null, int createCode = 0, byte[] metadata = null) : base(networker, networkBehavior, createCode, metadata) { Initialize(); }
		public Player_NetworkObject(NetWorker networker, uint serverId, FrameStream frame) : base(networker, serverId, frame) { Initialize(); }

		// DO NOT TOUCH, THIS GETS GENERATED PLEASE EXTEND THIS CLASS IF YOU WISH TO HAVE CUSTOM CODE ADDITIONS
	}
}
