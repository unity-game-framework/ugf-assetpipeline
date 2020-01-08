using System;
using System.Collections.Generic;
using UGF.CustomSettings.Editor;
using UnityEditor;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    [InitializeOnLoad]
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

        private static Dictionary<string, List<AssetProcessor>> m_assets = new Dictionary<string, List<AssetProcessor>>();

        static AssetProcessorSettings()
        {
            m_settings.Loaded += OnSettingsLoaded;
        }

        public static bool Contains(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            return m_assets.ContainsKey(guid);
        }

        public static void AddAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            m_assets.Add(guid, new List<AssetProcessor>());
            SaveSettings();
        }

        public static void RemoveAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            m_assets.Remove(guid);
            SaveSettings();
        }

        public static void AddProcessor(string guid, AssetProcessor processor)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            if (!m_assets.TryGetValue(guid, out List<AssetProcessor> processors))
            {
                processors = new List<AssetProcessor>();

                m_assets.Add(guid, processors);
            }

            processors.Add(processor);
            SaveSettings();
        }

        public static void RemoveProcessor(string guid, AssetProcessor processor)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            if (m_assets.TryGetValue(guid, out List<AssetProcessor> processors))
            {
                processors.Remove(processor);
                SaveSettings();
            }
        }

        public static bool TryGetProcessors(ICollection<AssetProcessor> processors, string guid)
        {
            if (processors == null) throw new ArgumentNullException(nameof(processors));
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            if (m_assets.TryGetValue(guid, out List<AssetProcessor> collection))
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

        private static void SaveSettings()
        {
            m_settings.Data.Assets.Clear();

            foreach (KeyValuePair<string, List<AssetProcessor>> pair in m_assets)
            {
                var info = new AssetProcessorSettingsData.AssetInfo
                {
                    Guid = pair.Key
                };

                info.Processors.AddRange(pair.Value);

                m_settings.Data.Assets.Add(info);
            }

            m_settings.SaveSettings();
        }

        private static void OnSettingsLoaded()
        {
            m_assets.Clear();

            for (int i = 0; i < m_settings.Data.Assets.Count; i++)
            {
                AssetProcessorSettingsData.AssetInfo info = m_settings.Data.Assets[i];

                if (!string.IsNullOrEmpty(info.Guid) && !m_assets.ContainsKey(info.Guid))
                {
                    m_assets.Add(info.Guid, new List<AssetProcessor>(info.Processors));
                }
            }
        }
    }
}
