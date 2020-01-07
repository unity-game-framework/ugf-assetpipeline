using System;
using System.Collections.Generic;
using System.Linq;
using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Types;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    [CustomEditor(typeof(AssetProcessorSettingsData))]
    internal class AssetProcessorSettingsDataEditor : UnityEditor.Editor
    {
        private SerializedProperty m_propertyActive;
        private SerializedProperty m_propertyAssets;
        private SearchField m_searchGUI;
        private string m_searchText;
        private TypesDropdown m_typesDropdown;
        private string m_typesDropdownCurrentAssetGuid;

        private void OnEnable()
        {
            m_propertyActive = serializedObject.FindProperty("m_active");
            m_propertyAssets = serializedObject.FindProperty("m_assets");

            m_typesDropdown = new TypesDropdown(ProcessorTypesCollector);
            m_typesDropdown.Selected += ProcessorTypeSelected;
        }

        public override void OnInspectorGUI()
        {
            if (m_searchGUI == null)
            {
                m_searchGUI = new SearchField();
            }

            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(m_propertyActive);

            DrawAssets(m_propertyAssets);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawAssets(SerializedProperty propertyAssets)
        {
            EditorGUILayout.Space();

            m_searchText = m_searchGUI.OnGUI(m_searchText);

            EditorGUILayout.Space();

            using (new EditorGUI.IndentLevelScope())
            {
                for (int i = 0; i < propertyAssets.arraySize; i++)
                {
                    SerializedProperty propertyAsset = propertyAssets.GetArrayElementAtIndex(i);

                    DrawAsset(propertyAsset);
                }

                if (propertyAssets.arraySize > 0)
                {
                    DrawLine();
                }
                else
                {
                    EditorGUILayout.HelpBox("There is no any assets. Use 'Assets/Create/UGF/AssetProcessor' menu to manager assets.", MessageType.Info);
                }
            }
        }

        private void DrawAsset(SerializedProperty propertyAsset)
        {
            SerializedProperty propertyGuid = propertyAsset.FindPropertyRelative("m_guid");
            SerializedProperty propertyProcessors = propertyAsset.FindPropertyRelative("m_processors");

            string path = AssetDatabase.GUIDToAssetPath(propertyGuid.stringValue);
            var asset = AssetDatabase.LoadAssetAtPath<Object>(path);

            propertyAsset.isExpanded = EditorGUILayout.InspectorTitlebar(propertyAsset.isExpanded, asset, true);

            if (propertyAsset.isExpanded)
            {
                using (new EditorGUI.DisabledScope(true))
                {
                    EditorGUILayout.ObjectField("Asset", asset, asset != null ? asset.GetType() : typeof(Object), false, null);
                }

                for (int i = 0; i < propertyProcessors.arraySize; i++)
                {
                    SerializedProperty propertyProcessor = propertyProcessors.GetArrayElementAtIndex(i);
                    string processorName = propertyProcessor.managedReferenceFullTypename.Split('.').LastOrDefault();

                    propertyProcessor.isExpanded = EditorGUILayout.Foldout(propertyProcessor.isExpanded, processorName, true);

                    if (propertyProcessor.isExpanded)
                    {
                        using (new EditorGUI.IndentLevelScope())
                        {
                            EditorIMGUIUtility.DrawSerializedPropertyChildren(serializedObject, propertyProcessor.propertyPath);
                        }
                    }
                }

                EditorGUILayout.Space();

                DrawAddProcessorButton(propertyGuid.stringValue);

                EditorGUILayout.Space();
            }
        }

        private void DrawAddProcessorButton(string guid)
        {
            Rect rect = EditorGUILayout.GetControlRect(false, 20F);

            rect.xMin = rect.width / 2F - 150F;
            rect.width = 300F;

            if (GUI.Button(rect, "Add Processor"))
            {
                m_typesDropdownCurrentAssetGuid = guid;
                m_typesDropdown.Show(rect);
            }
        }

        private static void DrawLine()
        {
            Color color = GUI.color;
            GUI.color = new Color(0.1F, 0.1F, 0.1F);

            Rect rect = EditorGUILayout.GetControlRect(false, 1F);

            GUI.DrawTexture(rect, Texture2D.whiteTexture);

            GUI.color = color;
        }

        private void ProcessorTypeSelected(Type processorType)
        {
            string guid = m_typesDropdownCurrentAssetGuid;
            var processor = (IAssetProcessor)Activator.CreateInstance(processorType);

            AssetProcessorSettings.Add(guid, processor);
        }

        private static IEnumerable<Type> ProcessorTypesCollector()
        {
            return TypeCache.GetTypesWithAttribute<AssetProcessorAttribute>();
        }
    }
}
