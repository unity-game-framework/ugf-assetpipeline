using System;
using System.Collections.Generic;
using UGF.CustomSettings.Editor;
using UnityEditor;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    public static class AssetProcessorSettings
    {
        public static bool Active
        {
            get { return m_settings.Data.Active; }
            set
            {
                m_settings.Data.Active = true;
                m_settings.SaveSettings();
            }
        }

        private static readonly CustomSettingsEditorPackage<AssetProcessorSettingsData> m_settings = new CustomSettingsEditorPackage<AssetProcessorSettingsData>
        (
            "UGF.AssetPipeline",
            "AssetProcessorSettings",
            CustomSettingsEditorUtility.DEFAULT_PACKAGE_EXTERNAL_FOLDER
        );

        static AssetProcessorSettings()
        {
        }

        public static bool Contains(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            return m_settings.Data.Assets.ContainsKey(guid);
        }

        public static bool Contains(IAssetProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            return m_settings.Data.Processors.Contains(processor);
        }

        public static void AddAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            m_settings.Data.Assets.Add(guid, new List<IAssetProcessor>());
            m_settings.SaveSettings();
        }

        public static void RemoveAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            m_settings.Data.Assets.Remove(guid);
            m_settings.SaveSettings();
        }

        public static void AddAssetProcessor(string guid, IAssetProcessor processor)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            if (!Contains(processor))
            {
                AddProcessor(processor);
            }

            if (!m_settings.Data.Assets.TryGetValue(guid, out List<IAssetProcessor> processors))
            {
                processors = new List<IAssetProcessor>();

                m_settings.Data.Assets.Add(guid, processors);
            }

            processors.Add(processor);
            m_settings.SaveSettings();
        }

        public static void RemoveAssetProcessor(string guid, IAssetProcessor processor)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            if (m_settings.Data.Assets.TryGetValue(guid, out List<IAssetProcessor> processors))
            {
                processors.Remove(processor);
                m_settings.SaveSettings();
            }
        }

        public static void AddProcessor(IAssetProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            m_settings.Data.Processors.Add(processor);
            m_settings.SaveSettings();
        }

        public static void RemoveProcessor(IAssetProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            foreach (KeyValuePair<string, List<IAssetProcessor>> pair in m_settings.Data.Assets)
            {
                pair.Value.Remove(processor);
            }

            m_settings.Data.Processors.Remove(processor);
            m_settings.SaveSettings();
        }

        public static void GetProcessors(ICollection<IAssetProcessor> processors)
        {
            if (processors == null) throw new ArgumentNullException(nameof(processors));

            for (int i = 0; i < m_settings.Data.Processors.Count; i++)
            {
                processors.Add(m_settings.Data.Processors[i]);
            }
        }

        public static bool TryGetProcessors(ICollection<IAssetProcessor> processors, string guid)
        {
            if (processors == null) throw new ArgumentNullException(nameof(processors));
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            if (m_settings.Data.Assets.TryGetValue(guid, out List<IAssetProcessor> collection))
            {
                for (int i = 0; i < collection.Count; i++)
                {
                    processors.Add(collection[i]);
                }

                return collection.Count > 0;
            }

            return false;
        }

        [SettingsProvider]
        private static SettingsProvider GetProvider()
        {
            return new CustomSettingsProvider<AssetProcessorSettingsData>("Project/UGF/Asset Processors", m_settings, SettingsScope.Project);
        }
    }
}
