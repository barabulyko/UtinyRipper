using System.Collections.Generic;
using uTinyRipper.Project;
using uTinyRipper.Classes.AudioMixers;
using uTinyRipper.Converters;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes
{
#warning TODO: not implemented
	public sealed class AudioMixer : NamedObject
	{
		public AudioMixer(AssetInfo assetInfo):
			base(assetInfo)
		{
		}

		/*private static int GetSerializedVersion(Version version)
		{
#warning TODO: serialized version acording to read version (current 2017.3.0f3)
			return 2;
		}*/

		public override void Read(AssetReader reader)
		{
			base.Read(reader);

			OutputGroup.Read(reader);
			MasterGroup.Read(reader);
			m_snapshots = reader.ReadAssetArray<PPtr<AudioMixerSnapshot>>();
			StartSnapshot.Read(reader);
			SuspendThreshold = reader.ReadSingle();
			EnableSuspend = reader.ReadBoolean();
			reader.AlignStream(AlignType.Align4);
			
			UpdateMode = reader.ReadInt32();
			reader.AlignStream(AlignType.Align4);
			
			MixerConstant.Read(reader);
			reader.AlignStream(AlignType.Align4);
			
		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			YAMLMappingNode node = base.ExportYAMLRoot(container);
			//node.AddSerializedVersion(GetSerializedVersion(container.Version));
			node.Add(OutputGroupName, OutputGroup.ExportYAML(container));
			node.Add(MasterGroupName, MasterGroup.ExportYAML(container));
			node.Add(SnapshotsName, Snapshots.ExportYAML(container));
			node.Add(StartSnapshotName, StartSnapshot.ExportYAML(container));
			node.Add(SuspendThresholdName, SuspendThreshold);
			node.Add(EnableSuspendName, EnableSuspend);
			node.Add(UpdateModeName, UpdateMode);
			node.Add(MixerConstantName, MixerConstant.ExportYAML(container));
			return node;
		}

		public IReadOnlyList<PPtr<AudioMixerSnapshot>> Snapshots => m_snapshots;
		public float SuspendThreshold { get; private set; }
		public bool EnableSuspend { get; private set; }
		public int UpdateMode { get; private set; }

		public const string OutputGroupName = "m_OutputGroup";
		public const string MasterGroupName = "m_MasterGroup";
		public const string SnapshotsName = "m_Snapshots";
		public const string StartSnapshotName = "m_StartSnapshot";
		public const string SuspendThresholdName = "m_SuspendThreshold";
		public const string EnableSuspendName = "m_EnableSuspend";
		public const string UpdateModeName = "m_UpdateMode";
		public const string MixerConstantName = "m_MixerConstant";

		public PPtr<AudioMixerGroup> OutputGroup;
		public PPtr<AudioMixerGroup> MasterGroup;
		public PPtr<AudioMixerSnapshot> StartSnapshot;
		public AudioMixerConstant MixerConstant;

		private PPtr<AudioMixerSnapshot>[] m_snapshots;
	}
}
