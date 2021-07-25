using System;
using UnityEditor.AssetImporters;

namespace UGF.AssetPipeline.Editor.Asset.Info
{
    public abstract class AssetInfoImporter : ScriptedImporter
    {
        public abstract Type InfoType { get; }

        public abstract object Load();
        public abstract void Save(object info);
    }
}
