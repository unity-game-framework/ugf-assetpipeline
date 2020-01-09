using System;
using UnityEditor.Experimental.AssetImporters;

namespace UGF.AssetPipeline.Editor.Asset.Info
{
    public abstract class AssetInfoImporter : ScriptedImporter
    {
        public abstract Type InfoType { get; }

        public abstract IAssetInfo Load();
        public abstract void Save(IAssetInfo info);
    }
}
