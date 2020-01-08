using System;
using System.Collections.Generic;
using UnityEngine;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    internal class AssetProcessorSettingsData : ScriptableObject
    {
        [SerializeField] private bool m_active = true;
        [SerializeField] private List<AssetInfo> m_assets = new List<AssetInfo>();

        public bool Active { get { return m_active; } set { m_active = value; } }
        public List<AssetInfo> Assets { get { return m_assets; } }

        [Serializable]
        public class AssetInfo
        {
            [SerializeField] private string m_guid;
            [SerializeField] private List<AssetProcessor> m_processors = new List<AssetProcessor>();

            public string Guid { get { return m_guid; } set { m_guid = value; } }
            public List<AssetProcessor> Processors { get { return m_processors; } }
        }
    }
}
