using System;
using UGF.EditorTools.Editor.IMGUI;
using UnityEditor;
using UnityEditor.AssetImporters;
using Object = UnityEngine.Object;

namespace UGF.AssetPipeline.Editor.Asset.Info
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(AssetInfoImporter), true)]
    public class AssetInfoImporterEditor : ScriptedImporterEditor
    {
        public string InfoName { get; private set; }

        protected override Type extraDataType { get; } = typeof(AssetInfoData);

        public override void OnEnable()
        {
            base.OnEnable();

            var importer = (AssetInfoImporter)target;

            InfoName = ObjectNames.NicifyVariableName(importer.InfoType.Name);
        }

        public override void OnInspectorGUI()
        {
            OnDrawImportSettings();
            OnDrawInfo();

            ApplyRevertGUI();
        }

        protected virtual void OnDrawImportSettings()
        {
            EditorIMGUIUtility.DrawDefaultInspector(serializedObject);
        }

        protected virtual void OnDrawInfo()
        {
            extraDataSerializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(InfoName, EditorStyles.boldLabel);

            EditorIMGUIUtility.DrawSerializedPropertyChildren(extraDataSerializedObject, "m_info");

            extraDataSerializedObject.ApplyModifiedProperties();
        }

        protected override void InitializeExtraDataInstance(Object extraData, int targetIndex)
        {
            var data = (AssetInfoData)extraData;
            var importer = (AssetInfoImporter)targets[targetIndex];
            object info = importer.Load();

            data.Info = info;
        }

        protected override void Apply()
        {
            base.Apply();

            for (int i = 0; i < extraDataTargets.Length; i++)
            {
                var data = (AssetInfoData)extraDataTargets[i];
                var importer = (AssetInfoImporter)targets[i];

                importer.Save(data.Info);
            }
        }
    }
}
