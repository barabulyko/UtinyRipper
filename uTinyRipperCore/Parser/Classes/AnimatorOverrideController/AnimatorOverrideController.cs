using System.Collections.Generic;
using uTinyRipper.Project;
using uTinyRipper.Classes.AnimatorOverrideControllers;
using uTinyRipper.YAML;
using uTinyRipper.Converters;

namespace uTinyRipper.Classes
{
	public sealed class AnimatorOverrideController : RuntimeAnimatorController
	{
		public AnimatorOverrideController(AssetInfo assetInfo) :
			base(assetInfo)
		{
		}

		public override void Read(AssetReader reader)
		{
			base.Read(reader);

			Controller.Read(reader);
			m_clips = reader.ReadAssetArray<AnimationClipOverride>();
		}

		public override IEnumerable<Object> FetchDependencies(IDependencyContext context)
		{
			foreach(Object asset in base.FetchDependencies(context))
			{
				yield return asset;
			}
			
			yield return context.FetchDependency(Controller, ControllerName);
			foreach (Object asset in context.FetchDependencies(Clips, ClipsName))
			{
				yield return asset;
			}
		}

		public override bool IsContainsAnimationClip(AnimationClip clip)
		{
			foreach (AnimationClipOverride overClip in Clips)
			{
				if(overClip.OriginalClip.IsAsset(File, clip))
				{
					return true;
				}
				else if (overClip.OverrideClip.IsAsset(File, clip))
				{
					return true;
				}
			}
			RuntimeAnimatorController baseController = Controller.FindAsset(File);
			if(baseController != null)
			{
				return baseController.IsContainsAnimationClip(clip);
			}
			return false;
		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			YAMLMappingNode node = base.ExportYAMLRoot(container);
			node.Add(ControllerName, Controller.ExportYAML(container));
			node.Add(ClipsName, Clips.ExportYAML(container));
			return node;
		}

		public override string ExportExtension => "overrideController";

		public IReadOnlyList<AnimationClipOverride> Clips => m_clips;

		public const string ControllerName = "m_Controller";
		public const string ClipsName = "m_Clips";

		public PPtr<RuntimeAnimatorController> Controller;

		private AnimationClipOverride[] m_clips;
	}
}
