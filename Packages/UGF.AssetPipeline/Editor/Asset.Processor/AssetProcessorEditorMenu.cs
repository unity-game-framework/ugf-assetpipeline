using UGF.AssetPipeline.Editor.Asset.Processor.Settings;
using UnityEditor;

namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    internal static class AssetProcessorEditorMenu
    {
        private const string ADD_ASSET_PROCESSOR_MENU_NAME = "Assets/Create/UGF/AssetPipeline/Add to Asset Processors";
        private const string REMOVE_ASSET_PROCESSOR_MENU_NAME = "Assets/Create/UGF/AssetPipeline/Remove from Asset Processors";

        [MenuItem(ADD_ASSET_PROCESSOR_MENU_NAME, false, 2000)]
        private static void AddAssetProcessorMenu()
        {
            string[] guids = Selection.assetGUIDs;

            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];

                AssetProcessorSettings.AddAsset(guid);
            }
        }

        [MenuItem(ADD_ASSET_PROCESSOR_MENU_NAME, true, 2000)]
        private static bool AddAssetProcessorValidate()
        {
            return !ContainsAll(Selection.assetGUIDs);
        }

        [MenuItem(REMOVE_ASSET_PROCESSOR_MENU_NAME, false, 2000)]
        private static void RemoveAssetProcessorMenu()
        {
            string[] guids = Selection.assetGUIDs;

            for (int i = 0; i < guids.Length; i++)
            {
                string guid = guids[i];

                AssetProcessorSettings.RemoveAsset(guid);
            }
        }

        [MenuItem(REMOVE_ASSET_PROCESSOR_MENU_NAME, true, 2000)]
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
