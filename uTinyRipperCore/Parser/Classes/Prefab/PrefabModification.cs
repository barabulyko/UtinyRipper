using System.Collections.Generic;
using uTinyRipper.Project;
using uTinyRipper.Converters;
using uTinyRipper.YAML;

namespace uTinyRipper.Classes.Prefabs
{
	public struct PrefabModification : IAssetReadable, IYAMLExportable
	{
		public void Read(AssetReader reader)
		{
			TransformParent.Read(reader);
			m_modifications = reader.ReadAssetArray<PropertyModification>();
			m_removedComponents = reader.ReadAssetArray<PPtr<Object>>();
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.Add(TransformParentName, TransformParent.ExportYAML(container));
			node.Add(ModificationsName, m_modifications == null ? YAMLSequenceNode.Empty : Modifications.ExportYAML(container));
			node.Add(RemovedComponentsName, m_removedComponents == null ? YAMLSequenceNode.Empty : RemovedComponents.ExportYAML(container));
			return node;
		}

		public IReadOnlyList<PropertyModification> Modifications => m_modifications;
		public IReadOnlyList<PPtr<Object>> RemovedComponents => m_removedComponents;

		public const string TransformParentName = "m_TransformParent";
		public const string ModificationsName = "m_Modifications";
		public const string RemovedComponentsName = "m_RemovedComponents";

		public PPtr<Transform> TransformParent;

		private PropertyModification[] m_modifications;
		private PPtr<Object>[] m_removedComponents;
	}
}
