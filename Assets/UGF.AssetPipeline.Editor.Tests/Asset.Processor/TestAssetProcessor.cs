using System;
using UGF.AssetPipeline.Editor.Asset.Processor;
using UnityEngine;

namespace UGF.AssetPipeline.Editor.Tests.Asset.Processor
{
    [Serializable, AssetProcessor]
    public class TestAssetProcessor : AssetProcessor
    {
        [SerializeField] private string m_name = "Test";

        public string Name { get { return m_name; } set { m_name = value; } }

        public override void OnImport(string path)
        {
            Debug.Log(path);
        }
    }
}
