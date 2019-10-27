﻿using uTinyRipper.Classes.Rigidbodies;
using uTinyRipper.Converters;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes
{
	public sealed class Rigidbody : Component
	{
		public Rigidbody(AssetInfo assetInfo):
			base(assetInfo)
		{
		}

		/// <summary>
		/// 1.5.0 and greater
		/// </summary>
		public static bool IsReadInterpolate(Version version)
		{
			return version.IsGreaterEqual(1, 5);
		}
		/// <summary>
		/// 3.0.0 and greater
		/// </summary>
		public static bool IsReadCollisionDetection(Version version)
		{
			return version.IsGreaterEqual(3);
		}
		
		/// <summary>
		/// Less than 3.2.0
		/// </summary>
		private static bool IsReadFreezeRotation(Version version)
		{
			return version.IsLess(3, 2);
		}
		/// <summary>
		/// 3.2.0 and greater
		/// </summary>
		private static bool IsAlign(Version version)
		{
			return version.IsGreaterEqual(3, 2);
		}

		private static int GetSerializedVersion(Version version)
		{
			if (version.IsGreaterEqual(3, 2))
			{
				return 2;
			}
			return 1;
		}

		public override void Read(AssetReader reader)
		{
			base.Read(reader);

			Mass = reader.ReadSingle();
			Drag = reader.ReadSingle();
			AngularDrag = reader.ReadSingle();
			UseGravity = reader.ReadBoolean();
			IsKinematic = reader.ReadBoolean();
			if (IsReadInterpolate(reader.Version))
			{
				Interpolate = (RigidbodyInterpolation)reader.ReadByte();
				if (IsAlign(reader.Version))
				{
					reader.AlignStream(AlignType.Align4);
				}
			}

			if (IsReadFreezeRotation(reader.Version))
			{
				bool freezeRotation = reader.ReadBoolean();
				Constraints = freezeRotation ? RigidbodyConstraints.FreezeRotation : RigidbodyConstraints.None;
			}
			else
			{
				Constraints = (RigidbodyConstraints)reader.ReadInt32();
			}
			if (IsReadCollisionDetection(reader.Version))
			{
				CollisionDetection = (CollisionDetectionMode)reader.ReadInt32();
			}
		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			YAMLMappingNode node = base.ExportYAMLRoot(container);
			node.AddSerializedVersion(GetSerializedVersion(container.ExportVersion));
			node.Add(MassName, Mass);
			node.Add(DragName, Drag);
			node.Add(AngularDragName, AngularDrag);
			node.Add(UseGravityName, UseGravity);
			node.Add(IsKinematicName, IsKinematic);
			node.Add(InterpolateName, (byte)Interpolate);
			node.Add(ConstraintsName, (int)Constraints);
			node.Add(CollisionDetectionName, (int)CollisionDetection);
			return node;
		}

		public float Mass { get; private set; }
		public float Drag { get; private set; }
		public float AngularDrag { get; private set; }
		public bool UseGravity { get; private set; }
		public bool IsKinematic { get; private set; }
		public RigidbodyInterpolation Interpolate { get; private set; }
		public RigidbodyConstraints Constraints { get; private set; }
		public CollisionDetectionMode CollisionDetection { get; private set; }

		public const string MassName = "m_Mass";
		public const string DragName = "m_Drag";
		public const string AngularDragName = "m_AngularDrag";
		public const string UseGravityName = "m_UseGravity";
		public const string IsKinematicName = "m_IsKinematic";
		public const string InterpolateName = "m_Interpolate";
		public const string ConstraintsName = "m_Constraints";
		public const string CollisionDetectionName = "m_CollisionDetection";
	}
}
