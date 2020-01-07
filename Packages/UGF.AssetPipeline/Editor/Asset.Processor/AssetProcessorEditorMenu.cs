using UGF.AssetPipeline.Editor.Asset.Processor.Settings;
using UnityEditor;

namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    internal static class AssetProcessorEditorMenu
    {
        [MenuItem("Assets/Create/UGF/AssetProcessor/Add to Asset Processors", false, 2000)]
        private static void AddAssetProcessorMenu()
        {
            string[] guids = Selection.assetGUIDs;

            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];

                AssetProcessorSettings.Add(guid);
            }
        }

        [MenuItem("Assets/Create/UGF/AssetProcessor/Add to Asset Processors", true, 2000)]
        private static bool AddAssetProcessorValidate()
        {
            return !ContainsAll(Selection.assetGUIDs);
        }

        [MenuItem("Assets/Create/UGF/AssetProcessor/Remove from Asset Processors", false, 2000)]
        private static void RemoveAssetProcessorMenu()
        {
            string[] guids = Selection.assetGUIDs;

            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];

                AssetProcessorSettings.Remove(guid);
            }
        }

        [MenuItem("Assets/Create/UGF/AssetProcessor/Remove from Asset Processors", true, 2000)]
        private static bool RemoveAssetProcessorValidate()
        {
            return ContainsAll(Selection.assetGUIDs);
        }

        private static bool ContainsAll(string[] guids)
        {
            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];

                if (!AssetProcessorSettings.Contains(guid))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
