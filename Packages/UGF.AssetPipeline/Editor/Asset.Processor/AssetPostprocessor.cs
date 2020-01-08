using System.Collections.Generic;
using UGF.AssetPipeline.Editor.Asset.Processor.Settings;
using UnityEditor;

namespace UGF.AssetPipeline.Editor.Asset.Processor
{
    internal class AssetPostprocessor : UnityEditor.AssetPostprocessor
    {
        private static List<IAssetProcessor> m_processors = new List<IAssetProcessor>();

        private static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
        {
            if (AssetProcessorSettings.Active)
            {
                HandleImported(importedAssets);
                HandleDeleted(deletedAssets);
                HandleMoved(movedFromAssetPaths, movedAssets);
            }
        }

        private static void HandleImported(IReadOnlyList<string> paths)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];
                string guid = AssetDatabase.AssetPathToGUID(path);

                if (AssetProcessorSettings.TryGetProcessors(m_processors, guid))
                {
                    for (int p = 0; p < m_processors.Count; p++)
                    {
                        m_processors[p].OnImport(path);
                    }
                }

                m_processors.Clear();
            }
        }

        private static void HandleDeleted(IReadOnlyList<string> paths)
        {
            for (int i = 0; i < paths.Count; i++)
            {
                string path = paths[i];
                string guid = AssetDatabase.AssetPathToGUID(path);

                if (AssetProcessorSettings.TryGetProcessors(m_processors, guid))
                {
                    for (int p = 0; p < m_processors.Count; p++)
                    {
                        m_processors[p].OnDelete(path);
                    }
                }

                m_processors.Clear();
            }
        }

        private static void HandleMoved(IReadOnlyList<string> pathsFrom, IReadOnlyList<string> pathsTo)
        {
            for (int i = 0; i < pathsTo.Count; i++)
            {
                string pathTo = pathsTo[i];
                string pathFrom = pathsFrom[i];
                string guid = AssetDatabase.AssetPathToGUID(pathTo);

                if (AssetProcessorSettings.TryGetProcessors(m_processors, guid))
                {
                    for (int p = 0; p < m_processors.Count; p++)
                    {
                        m_processors[p].OnMoved(pathFrom, pathTo);
                    }
                }

                m_processors.Clear();
            }
        }
    }
}
