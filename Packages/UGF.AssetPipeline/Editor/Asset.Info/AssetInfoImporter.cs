using System;


namespace UGF.AssetPipeline.Editor.Asset.Info
{
    public abstract class AssetInfoImporter : UnityEditor.AssetImporters.ScriptedImporter
    {
        public abstract Type InfoType { get; }

        public abstract object Load();
        public abstract void Save(object info);
    }
}
