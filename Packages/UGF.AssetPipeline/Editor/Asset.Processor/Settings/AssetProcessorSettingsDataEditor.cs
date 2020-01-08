using UGF.CustomSettings.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    [CustomEditor(typeof(AssetProcessorSettingsData))]
    internal class AssetProcessorSettingsDataEditor : UnityEditor.Editor
    {
        private SerializedProperty m_propertyActive;
        private SerializedProperty m_propertyAssets;
        private ReorderableList m_list;
        private Styles m_styles;

        private class Styles
        {
            public GUIStyle FoldoutTitlebar { get; } = "IN Title";
            public GUIStyle IconButton { get; } = "IconButton";
            public GUIStyle MenuIcon { get; } = "PaneOptions";
            public GUIContent AddIcon { get; } = EditorGUIUtility.IconContent("Toolbar Plus");
        }

        private void OnEnable()
        {
            m_propertyActive = serializedObject.FindProperty("m_active");
            m_propertyAssets = serializedObject.FindProperty("m_assets");

            m_list = new ReorderableList(serializedObject, m_propertyAssets);
            m_list.drawHeaderCallback = OnDrawHeader;
            m_list.drawElementCallback = OnDrawElement;
            m_list.elementHeightCallback = OnElementHeight;
            m_list.onAddCallback = OnAdd;
        }

        public override void OnInspectorGUI()
        {
            if (m_styles == null)
            {
                m_styles = new Styles();
            }

            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(m_propertyActive);
            EditorGUILayout.Space();

            using (new CustomSettingsInspectorScope())
            {
                m_list.DoLayoutList();
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void OnDrawHeader(Rect rect)
        {
            GUI.Label(rect, $"{m_propertyAssets.displayName} (Size: {m_propertyAssets.arraySize})", EditorStyles.boldLabel);
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            SerializedProperty propertyElement = m_list.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty propertyGuid = propertyElement.FindPropertyRelative("m_guid");

            float height = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            var rectAsset = new Rect(rect.x, rect.y + space, rect.width, height);

            string guid = propertyGuid.stringValue;
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);

            asset = EditorGUI.ObjectField(rectAsset, asset, typeof(Object), false);

            path = AssetDatabase.GetAssetPath(asset);
            guid = AssetDatabase.AssetPathToGUID(path);

            propertyGuid.stringValue = guid;
        }

        private float OnElementHeight(int index)
        {
            float height = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            return height + space * 2F;
        }

        private void OnAdd(ReorderableList list)
        {
            list.serializedProperty.InsertArrayElementAtIndex(list.serializedProperty.arraySize);

            SerializedProperty propertyElement = list.serializedProperty.GetArrayElementAtIndex(list.serializedProperty.arraySize - 1);
            SerializedProperty propertyGuid = propertyElement.FindPropertyRelative("m_guid");
            SerializedProperty propertyProcessors = propertyElement.FindPropertyRelative("m_processors");

            propertyGuid.stringValue = string.Empty;
            propertyProcessors.ClearArray();

            list.serializedProperty.serializedObject.ApplyModifiedProperties();
        }

        private static void DrawLine()
        {
            Color color = GUI.color;
            GUI.color = new Color(0.1F, 0.1F, 0.1F);

            Rect rect = EditorGUILayout.GetControlRect(false, 1F);

            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = color;
        }
    }
}
