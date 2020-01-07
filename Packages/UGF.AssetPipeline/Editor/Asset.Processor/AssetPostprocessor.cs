using System.Collections.Generic;
using UGF.AssetPipeline.Editor.Asset.Processor.Settings;
using UnityEditor;

namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    internal class AssetPostprocessor : UnityEditor.AssetPostprocessor
    {
        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (AssetProcessorSettings.Active)
            {
                HandleImported(importedAssets);
                HandleDeleted(deletedAssets);
                HandleMoved(movedFromAssetPaths, movedAssets);
            }
        }

        private static void HandleImported(string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                string guid = AssetDatabase.AssetPathToGUID(path);

                if (AssetProcessorSettings.TryGetAllOrdered(guid, out IReadOnlyList<IAssetProcessor> processors))
                {
                    for (int p = 0; p < processors.Count; p++)
                    {
                        processors[p].OnImport(path);
                    }
                }
            }
        }

        private static void HandleDeleted(string[] paths)
        {
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                string guid = AssetDatabase.AssetPathToGUID(path);

                if (AssetProcessorSettings.TryGetAllOrdered(guid, out IReadOnlyList<IAssetProcessor> processors))
                {
                    for (int p = 0; p < processors.Count; p++)
                    {
                        processors[p].OnDelete(path);
                    }
                }
            }
        }

        private static void HandleMoved(string[] pathsFrom, string[] pathsTo)
        {
            for (int i = 0; i < pathsTo.Length; i++)
            {
                string pathTo = pathsTo[i];
                string pathFrom = pathsFrom[i];
                string guid = AssetDatabase.AssetPathToGUID(pathTo);

                if (AssetProcessorSettings.TryGetAllOrdered(guid, out IReadOnlyList<IAssetProcessor> processors))
                {
                    for (int p = 0; p < processors.Count; p++)
                    {
                        processors[p].OnMoved(pathFrom, pathTo);
                    }
                }
            }
        }
    }
}
