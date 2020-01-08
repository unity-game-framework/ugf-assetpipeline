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

            return TryGetProcessorId(processor, out _);
        }

        public static void AddAsset(string guid)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            m_settings.Data.Assets.Add(guid, new List<string>());
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

            if (!TryGetProcessorId(processor, out string id))
            {
                id = Guid.NewGuid().ToString("N");

                m_settings.Data.Processors.Add(id, processor);
            }

            if (!m_settings.Data.Assets.TryGetValue(guid, out List<string> processors))
            {
                processors = new List<string>();

                m_settings.Data.Assets.Add(guid, processors);
            }

            processors.Add(id);
            m_settings.SaveSettings();
        }

        public static void RemoveAssetProcessor(string guid, IAssetProcessor processor)
        {
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            if (TryGetProcessorId(processor, out string id))
            {
                if (m_settings.Data.Assets.TryGetValue(guid, out List<string> processors))
                {
                    processors.Remove(id);
                    m_settings.SaveSettings();
                }
            }
        }

        public static void AddProcessor(IAssetProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            if (TryGetProcessorId(processor, out string id))
            {
                throw new ArgumentException($"The specified processor already exists: '{processor}'.");
            }

            id = Guid.NewGuid().ToString("N");

            m_settings.Data.Processors.Add(id, processor);
            m_settings.SaveSettings();
        }

        public static void RemoveProcessor(IAssetProcessor processor)
        {
            if (processor == null) throw new ArgumentNullException(nameof(processor));

            if (TryGetProcessorId(processor, out string id))
            {
                foreach (KeyValuePair<string, List<string>> pair in m_settings.Data.Assets)
                {
                    pair.Value.Remove(id);
                }

                m_settings.Data.Processors.Remove(id);
                m_settings.SaveSettings();
            }
        }

        public static void GetProcessors(ICollection<IAssetProcessor> processors)
        {
            if (processors == null) throw new ArgumentNullException(nameof(processors));

            foreach (KeyValuePair<string, IAssetProcessor> pair in m_settings.Data.Processors)
            {
                processors.Add(pair.Value);
            }
        }

        public static bool TryGetProcessors(ICollection<IAssetProcessor> processors, string guid)
        {
            if (processors == null) throw new ArgumentNullException(nameof(processors));
            if (string.IsNullOrEmpty(guid)) throw new ArgumentException("Value cannot be null or empty.", nameof(guid));

            if (m_settings.Data.Assets.TryGetValue(guid, out List<string> ids))
            {
                for (int i = 0; i < ids.Count; i++)
                {
                    string id = ids[i];

                    if (m_settings.Data.Processors.TryGetValue(id, out IAssetProcessor processor))
                    {
                        processors.Add(processor);
                    }
                }

                return ids.Count > 0;
            }

            return false;
        }

        private static bool TryGetProcessorId(IAssetProcessor processor, out string id)
        {
            foreach (KeyValuePair<string, IAssetProcessor> pair in m_settings.Data.Processors)
            {
                if (pair.Value == processor)
                {
                    id = pair.Key;
                    return true;
                }
            }

            id = null;
            return false;
        }

        [SettingsProvider]
        private static SettingsProvider GetProvider()
        {
            return new CustomSettingsProvider<AssetProcessorSettingsData>("Project/UGF/Asset Processors", m_settings, SettingsScope.Project);
        }
    }
}
