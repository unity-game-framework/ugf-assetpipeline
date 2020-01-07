using UGF.AssetPipeline.Editor.Asset.Processor.Settings;

namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    internal class AssetPostprocessor : UnityEditor.AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (AssetProcessorSettings.Active)
            {
            }
        }
    }
}
