using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    internal class AssetProcessorSettingsData : ScriptableObject, ISerializationCallbackReceiver
    {
        [SerializeField] private bool m_active = true;
        [SerializeField] private List<AssetInfo> m_assets = new List<AssetInfo>();

        public bool Active { get { return m_active; } set { m_active = value; } }
        public List<AssetInfo> Assets { get { return m_assets; } }

        public event Action BeforeSerialize;
        public event Action AfterDeserialize;

        [Serializable]
        public class AssetInfo
        {
            [SerializeField] private bool m_active = true;
            [SerializeField] private string m_guid;
            [SerializeField] private List<ProcessorInfo> m_processors = new List<ProcessorInfo>();

            public bool Active { get { return m_active; } set { m_active = value; } }
            public string Guid { get { return m_guid; } set { m_guid = value; } }
            public List<ProcessorInfo> Processors { get { return m_processors; } }
        }

        [Serializable]
        public class ProcessorInfo
        {
            [SerializeField] private bool m_active = true;
            [SerializeField] private AssetProcessor m_processor;

            public bool Active { get { return m_active; } set { m_active = value; } }
            public AssetProcessor Processor { get { return m_processor; } set { m_processor = value; } }
        }

        void ISerializationCallbackReceiver.OnBeforeSerialize()
        {
            BeforeSerialize?.Invoke();
        }

        void ISerializationCallbackReceiver.OnAfterDeserialize()
        {
            AfterDeserialize?.Invoke();
        }
    }
}
