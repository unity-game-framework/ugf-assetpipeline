using UGF.AssetPipeline.Editor.Asset.Info;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace UGF.AssetPipeline.Editor.Tests.Asset.Info
{
    [ScriptedImporter(0, "info")]
    public class TestAssetInfoImporter : AssetInfoImporter<TestAssetInfo>
    {
        [SerializeField] private int m_intValue = 15;

        public int IntValue { get { return m_intValue; } set { m_intValue = value; } }
    }
}
