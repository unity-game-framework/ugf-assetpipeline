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
            public GUIStyle IconButton { get; } = "IconButton";
            public GUIContent AddIcon { get; } = EditorGUIUtility.IconContent("Toolbar Plus");
            public GUIContent RemoveIcon { get; } = EditorGUIUtility.IconContent("Toolbar Minus");
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
            SerializedProperty propertyActive = propertyElement.FindPropertyRelative("m_active");
            SerializedProperty propertyGuid = propertyElement.FindPropertyRelative("m_guid");
            SerializedProperty propertyProcessors = propertyElement.FindPropertyRelative("m_processors");

            float line = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;

            var rectFoldout = new Rect(rect.x + space, rect.y + space, line, line);
            var rectActive = new Rect(rectFoldout.xMax, rectFoldout.y, line, line);
            var rectField = new Rect(rectActive.xMax, rectActive.y, rect.width - line * 2 - space, line);

            propertyElement.isExpanded = GUI.Toggle(rectFoldout, propertyElement.isExpanded, GUIContent.none, EditorStyles.foldout);

            EditorGUI.PropertyField(rectActive, propertyActive, GUIContent.none);

            ObjectField(rectField, propertyGuid);

            if (propertyElement.isExpanded)
            {
                var rectProcessorActive = new Rect(rectActive.x, rectActive.y, line, line);
                var rectProcessor = new Rect(rectProcessorActive.xMax, rectField.y, rectField.width - line - space, rectField.height);
                var rectMenu = new Rect(rectProcessor.xMax + space, rectProcessor.y + 1F, line, line);

                for (int i = 0; i < propertyProcessors.arraySize; i++)
                {
                    rectProcessorActive.y += line + space;
                    rectProcessor.y += line + space;
                    rectMenu.y += line + space;

                    SerializedProperty propertyProcessor = propertyProcessors.GetArrayElementAtIndex(i);
                    SerializedProperty propertyProcessorActive = propertyProcessor.FindPropertyRelative("m_active");
                    SerializedProperty propertyProcessorValue = propertyProcessor.FindPropertyRelative("m_processor");

                    EditorGUI.PropertyField(rectProcessorActive, propertyProcessorActive, GUIContent.none);
                    EditorGUI.PropertyField(rectProcessor, propertyProcessorValue, GUIContent.none);

                    if (GUI.Button(rectMenu, m_styles.RemoveIcon, m_styles.IconButton))
                    {
                        propertyProcessors.DeleteArrayElementAtIndex(i);
                    }
                }

                var rectAdd = new Rect(rect.xMax - line, rectProcessor.y + line + space, line, line);

                if (GUI.Button(rectAdd, m_styles.AddIcon, m_styles.IconButton))
                {
                    propertyProcessors.InsertArrayElementAtIndex(propertyProcessors.arraySize);

                    SerializedProperty propertyProcessor = propertyProcessors.GetArrayElementAtIndex(propertyProcessors.arraySize - 1);

                    propertyProcessor.objectReferenceValue = null;
                    propertyProcessor.serializedObject.ApplyModifiedProperties();
                }
            }
        }

        private float OnElementHeight(int index)
        {
            float line = EditorGUIUtility.singleLineHeight;
            float space = EditorGUIUtility.standardVerticalSpacing;
            float field = line + space * 2;

            SerializedProperty propertyElement = m_list.serializedProperty.GetArrayElementAtIndex(index);
            SerializedProperty propertyProcessors = propertyElement.FindPropertyRelative("m_processors");

            float height = field;

            if (propertyElement.isExpanded)
            {
                height += field;
                height += field * propertyProcessors.arraySize;
            }

            return height;
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

        private static void ObjectField(Rect rect, SerializedProperty serializedProperty)
        {
            string guid = serializedProperty.stringValue;
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);

            asset = EditorGUI.ObjectField(rect, asset, typeof(Object), false);

            path = AssetDatabase.GetAssetPath(asset);
            guid = AssetDatabase.AssetPathToGUID(path);

            serializedProperty.stringValue = guid;
        }
    }
}
