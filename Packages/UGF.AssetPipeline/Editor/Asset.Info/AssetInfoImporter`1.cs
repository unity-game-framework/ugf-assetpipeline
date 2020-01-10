using System;
using UnityEditor;
using UnityEditor.Experimental.AssetImporters;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGF.AssetPipeline.Editor.Asset.Info
{
    public abstract class AssetInfoImporter<TInfo> : AssetInfoImporter where TInfo : class, IAssetInfo, new()
    {
        public override Type InfoType { get; } = typeof(TInfo);

        public override void OnImportAsset(AssetImportContext context)
        {
            TInfo info = LoadInfo();
            Object asset = OnCreateAsset(info);

            context.AddObjectToAsset("main", asset);
            context.SetMainObject(asset);
        }

        public sealed override IAssetInfo Load()
        {
            return LoadInfo();
        }

        public sealed override void Save(IAssetInfo info)
        {
            SaveInfo((TInfo)info);
        }

        protected virtual TInfo LoadInfo()
        {
            var info = AssetInfoEditorUtility.LoadInfo<TInfo>(assetPath);

            return info;
        }

        protected virtual void SaveInfo(TInfo info)
        {
            AssetInfoEditorUtility.SaveInfo(assetPath, info);
        }

        protected virtual Object OnCreateAsset(TInfo info)
        {
            string text = EditorJsonUtility.ToJson(info);
            var asset = new TextAsset(text);

            return asset;
        }
    }
}
