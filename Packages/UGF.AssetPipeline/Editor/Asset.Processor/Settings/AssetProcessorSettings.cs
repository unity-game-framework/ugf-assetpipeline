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

        private static readonly Dictionary<string, AssetInfo> m_assets = new Dictionary<string, AssetInfo>();

        private class AssetInfo
        {
            public bool IsActive { get; set; } = true;
            public List<AssetProcessorSettingsData.ProcessorInfo> ProcessorsInfos { get; set; } = new List<AssetProcessorSettingsData.ProcessorInfo>();
        }

        static AssetProcessorSettings()
        {
            m_settings.Loaded += LoadSettings;
            m_settings.Data.AfterDeserialize += LoadSettings;
        }

        public static bool Contains(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            return m_assets.ContainsKey(guid);
        }

        public static bool IsActive(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            return m_assets.TryGetValue(guid, out AssetInfo info) && info.IsActive;
        }

        public static void AddAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            m_assets.Add(guid, new AssetInfo());
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

            if (!m_assets.TryGetValue(guid, out AssetInfo info))
            {
                info = new AssetInfo();

                m_assets.Add(guid, info);
            }

            info.ProcessorsInfos.Add(new AssetProcessorSettingsData.ProcessorInfo
            {
                Active = true,
                Processor = processor
            });

            SaveSettings();
        }

        public static void RemoveProcessor(string guid, AssetProcessor processor)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            if (m_assets.TryGetValue(guid, out AssetInfo info))
            {
                for (int i = 0; i < info.ProcessorsInfos.Count; i++)
                {
                    AssetProcessorSettingsData.ProcessorInfo processorInfo = info.ProcessorsInfos[i];

                    if (processorInfo.Processor == processor)
                    {
                        info.ProcessorsInfos.RemoveAt(i);
                        break;
                    }
                }

                SaveSettings();
            }
        }

        public static void GetActiveProcessors(ICollection<AssetProcessor> processors, string guid)
        {
            if (processors == null) throw new ArgumentNullException(nameof(processors));
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            if (m_assets.TryGetValue(guid, out AssetInfo info))
            {
                for (int i = 0; i < info.ProcessorsInfos.Count; i++)
                {
                    AssetProcessorSettingsData.ProcessorInfo processorInfo = info.ProcessorsInfos[i];

                    if (processorInfo.Active)
                    {
                        processors.Add(processorInfo.Processor);
                    }
                }
            }
        }

        public static void GetProcessors(ICollection<AssetProcessor> processors, string guid)
        {
            if (processors == null) throw new ArgumentNullException(nameof(processors));
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            if (m_assets.TryGetValue(guid, out AssetInfo info))
            {
                for (int i = 0; i < info.ProcessorsInfos.Count; i++)
                {
                    processors.Add(info.ProcessorsInfos[i].Processor);
                }
            }
        }

        private static void SaveSettings()
        {
            m_settings.Data.Assets.Clear();

            foreach (KeyValuePair<string, AssetInfo> pair in m_assets)
            {
                var info = new AssetProcessorSettingsData.AssetInfo
                {
                    Active = pair.Value.IsActive,
                    Guid = pair.Key
                };

                info.Processors.AddRange(pair.Value.ProcessorsInfos);

                m_settings.Data.Assets.Add(info);
            }

            m_settings.SaveSettings();
        }

        private static void LoadSettings()
        {
            m_assets.Clear();

            for (int i = 0; i < m_settings.Data.Assets.Count; i++)
            {
                AssetProcessorSettingsData.AssetInfo info = m_settings.Data.Assets[i];

                if (!string.IsNullOrEmpty(info.Guid) && !m_assets.ContainsKey(info.Guid))
                {
                    m_assets.Add(info.Guid, new AssetInfo
                    {
                        IsActive = info.Active,
                        ProcessorsInfos = new List<AssetProcessorSettingsData.ProcessorInfo>(info.Processors)
                    });
                }
            }
        }

        [SettingsProvider]
        private static SettingsProvider GetProvider()
        {
            return new CustomSettingsProvider<AssetProcessorSettingsData>("Project/UGF/Asset Processors", m_settings, SettingsScope.Project);
        }
    }
}
