using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    internal class AssetProcessorSettingsData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private bool m_active = true;
        [SerializeField] private List<AssetInfo> m_assets = new List<AssetInfo>();
        [SerializeField] private List<ProcessorInfo> m_processors = new List<ProcessorInfo>();

        public bool Active { get { return m_active; } set { m_active = value; } }
        public Dictionary<string, List<string>> Assets { get; } = new Dictionary<string, List<string>>();
        public Dictionary<string, IAssetProcessor> Processors { get; } = new Dictionary<string, IAssetProcessor>();

        [Serializable]
        private class AssetInfo
        {
            [SerializeField] private string m_guid;
            [SerializeField] private List<string> m_processors = new List<string>();

            public string Guid { get { return m_guid; } set { m_guid = value; } }
            public List<string> Processors { get { return m_processors; } }
        }

        [Serializable]
        private class ProcessorInfo
        {
            [SerializeField] private string m_id;
            [SerializeReference] private IAssetProcessor m_processor;

            public string Id { get { return m_id; } set { m_id = value; } }
            public IAssetProcessor Processor { get { return m_processor; } set { m_processor = value; } }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            m_assets.Clear();
            m_processors.Clear();

            foreach (KeyValuePair<string, List<string>> pair in Assets)
            {
                var info = new AssetInfo
                {
                    Guid = pair.Key
                };

                info.Processors.AddRange(pair.Value);

                m_assets.Add(info);
            }

            foreach (KeyValuePair<string, IAssetProcessor> pair in Processors)
            {
                var info = new ProcessorInfo
                {
                    Id = pair.Key,
                    Processor = pair.Value
                };

                m_processors.Add(info);
            }
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            Assets.Clear();
            Processors.Clear();

            for (int i = 0; i < m_assets.Count; i++)
            {
                AssetInfo info = m_assets[i];

                Assets.Add(info.Guid, new List<string>(info.Processors));
            }

            for (int i = 0; i < m_processors.Count; i++)
            {
                ProcessorInfo info = m_processors[i];

                Processors.Add(info.Id, info.Processor);
            }
        }
    }
}
