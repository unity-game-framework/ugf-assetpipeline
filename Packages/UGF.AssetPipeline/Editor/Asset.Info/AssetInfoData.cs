﻿using UnityEngine;

namespace UGF.AssetPipeline.Editor.Asset.Info
{
    internal class AssetInfoData : ScriptableObject
    {
        [SerializeReference] private IAssetInfo m_info;

        public IAssetInfo Info { get { return m_info; } set { m_info = value; } }
    }
}
