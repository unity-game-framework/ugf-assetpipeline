using UnityEngine;

namespace UGF.AssetPipeline.Editor.Asset.Info
{
    internal class AssetInfoData : ScriptableObject
    {
        [SerializeReference] private object m_info;

        public object Info { get { return m_info; } set { m_info = value; } }
    }
}
