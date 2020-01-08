using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    internal class AssetProcessorSettingsData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private bool m_active = true;
        [SerializeField] private List<AssetInfo> m_assets = new List<AssetInfo>();
        [SerializeReference] private List<IAssetProcessor> m_processors = new List<IAssetProcessor>();

        public bool Active { get { return m_active; } set { m_active = value; } }
        public Dictionary<string, List<IAssetProcessor>> Assets { get; } = new Dictionary<string, List<IAssetProcessor>>();
        public List<IAssetProcessor> Processors { get { return m_processors; } }

        [Serializable]
        private class AssetInfo
        {
            [SerializeField] private string m_guid;
            [SerializeReference] private List<IAssetProcessor> m_processors = new List<IAssetProcessor>();

            public string Guid { get { return m_guid; } set { m_guid = value; } }
            public List<IAssetProcessor> Processors { get { return m_processors; } }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            m_assets.Clear();

            foreach (KeyValuePair<string, List<IAssetProcessor>> pair in Assets)
            {
                var info = new AssetInfo
                {
                    Guid = pair.Key
                };

                info.Processors.AddRange(pair.Value);

                m_assets.Add(info);
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Assets.Clear();

            for (int i = 0; i < m_assets.Count; i++)
            {
                AssetInfo info = m_assets[i];

                Assets.Add(info.Guid, new List<IAssetProcessor>(info.Processors));
            }
        }
    }
}
