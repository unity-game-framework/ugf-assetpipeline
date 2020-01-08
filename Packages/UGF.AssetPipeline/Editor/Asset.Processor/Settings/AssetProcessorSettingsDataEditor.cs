using System;
using System.Collections.Generic;
using System.Linq;
using UGF.EditorTools.Editor.IMGUI;
using UGF.EditorTools.Editor.IMGUI.Types;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace UGF.AssetPipeline.Editor.Asset.Processor.Settings
{
    [CustomEditor(typeof(AssetProcessorSettingsData))]
    internal class AssetProcessorSettingsDataEditor : UnityEditor.Editor
    {
        private SerializedProperty m_propertyActive;
        private SerializedProperty m_propertyAssets;
        private SerializedProperty m_propertyProcessors;
        private TypesDropdown m_processorsDropdown;
        private int m_tab;
        private Styles m_styles;
        private readonly List<IAssetProcessor> m_processors = new List<IAssetProcessor>();

        private class Styles
        {
            public GUIStyle TabStyle { get; } = "LargeButton";
            public GUIContent[] TabNames { get; } = { new GUIContent("Assets"), new GUIContent("Processors") };
            public GUIStyle AddProcessorButton { get; } = "AC Button";
            public GUIContent AddProcessorButtonContent { get; } = new GUIContent("Add Processor");
            public GUIStyle FoldoutTitlebar { get; } = "IN Title";
            public GUIStyle ProcessorElement { get; } = "Box";
            public GUIStyle IconButton { get; } = "IconButton";
            public GUIStyle MenuIcon { get; } = "PaneOptions";
            public GUIContent AddIcon { get; } = EditorGUIUtility.IconContent("Toolbar Plus");
        }

        private void OnEnable()
        {
            m_propertyActive = serializedObject.FindProperty("m_active");
            m_propertyAssets = serializedObject.FindProperty("m_assets");
            m_propertyProcessors = serializedObject.FindProperty("m_processors");

            m_processorsDropdown = new TypesDropdown(() => TypeCache.GetTypesWithAttribute<AssetProcessorAttribute>());
            m_processorsDropdown.Selected += ProcessorTypeSelected;
        }

        public override void OnInspectorGUI()
        {
            if (m_styles == null)
            {
                m_styles = new Styles();
            }

            serializedObject.UpdateIfRequiredOrScript();

            EditorGUILayout.PropertyField(m_propertyActive);

            DrawTabs();

            switch (m_tab)
            {
                case 0:
                {
                    DrawAssetTab();
                    break;
                }
                case 1:
                {
                    DrawProcessorTab();
                    break;
                }
            }

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawTabs()
        {
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                m_tab = GUILayout.Toolbar(m_tab, m_styles.TabNames, m_styles.TabStyle);

                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.Space();
        }

        private void DrawAssetTab()
        {
            for (int i = 0; i < m_propertyAssets.arraySize; i++)
            {
                SerializedProperty propertyAsset = m_propertyAssets.GetArrayElementAtIndex(i);
                SerializedProperty propertyGuid = propertyAsset.FindPropertyRelative("m_guid");
                SerializedProperty propertyProcessors = propertyAsset.FindPropertyRelative("m_processors");

                string guid = propertyGuid.stringValue;
                string path = AssetDatabase.GUIDToAssetPath(guid);
                var asset = AssetDatabase.LoadAssetAtPath<Object>(path);
                string assetName = asset != null ? ObjectNames.GetInspectorTitle(asset) : "Missing";

                using (new EditorGUILayout.HorizontalScope(m_styles.FoldoutTitlebar))
                {
                    propertyAsset.isExpanded = EditorGUILayout.Foldout(propertyAsset.isExpanded, assetName, true);

                    if (DrawMenuButton(GUIContent.none, m_styles.MenuIcon))
                    {
                        ShowAssetMenu(guid);
                    }
                }

                if (propertyAsset.isExpanded)
                {
                    EditorGUILayout.Space(2.5F);

                    using (new EditorGUI.IndentLevelScope())
                    {
                        using (new EditorGUI.DisabledScope(true))
                        {
                            Type assetType = asset != null ? asset.GetType() : typeof(Object);

                            EditorGUILayout.ObjectField(asset, assetType, false);
                        }

                        for (int p = 0; p < propertyProcessors.arraySize; p++)
                        {
                            SerializedProperty propertyProcessor = propertyProcessors.GetArrayElementAtIndex(p);
                            string processorName = GetProcessorName(propertyProcessor);

                            using (new EditorGUILayout.HorizontalScope())
                            {
                                EditorGUILayout.LabelField(processorName);


                                if (DrawMenuButton(GUIContent.none, m_styles.MenuIcon))
                                {
                                    ShowAssetProcessorMenu(guid, p);
                                }
                            }
                        }

                        if (propertyProcessors.arraySize == 0)
                        {
                        }
                    }

                    EditorGUILayout.Space(2.5F);
                }
            }

            if (m_propertyAssets.arraySize > 0)
            {
                DrawLine();
            }
            else
            {
                EditorGUILayout.HelpBox("There is no any assets.", MessageType.Info);
            }
        }

        private void DrawProcessorTab()
        {
            for (int i = 0; i < m_propertyProcessors.arraySize; i++)
            {
                SerializedProperty propertyProcessor = m_propertyProcessors.GetArrayElementAtIndex(i);
                string processorName = GetProcessorName(propertyProcessor);

                using (new EditorGUILayout.HorizontalScope(m_styles.FoldoutTitlebar))
                {
                    propertyProcessor.isExpanded = EditorGUILayout.Foldout(propertyProcessor.isExpanded, processorName, true);

                    if (DrawMenuButton(GUIContent.none, m_styles.MenuIcon))
                    {
                        ShowProcessorMenu(i);
                    }
                }

                if (propertyProcessor.isExpanded)
                {
                    EditorGUILayout.Space(2.5F);

                    using (new EditorGUI.IndentLevelScope())
                    {
                        EditorIMGUIUtility.DrawSerializedPropertyChildren(serializedObject, propertyProcessor.propertyPath);
                    }

                    EditorGUILayout.Space(2.5F);
                }
            }

            if (m_propertyProcessors.arraySize > 0)
            {
                DrawLine();
            }
            else
            {
                EditorGUILayout.HelpBox("There is no any processors.", MessageType.Info);
            }

            DrawAddProcessorButton();
        }

        private void DrawAddProcessorButton()
        {
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                Rect rect = GUILayoutUtility.GetRect(m_styles.AddProcessorButtonContent, m_styles.AddProcessorButton);

                if (GUI.Button(rect, m_styles.AddProcessorButtonContent, m_styles.AddProcessorButton))
                {
                    m_processorsDropdown.Show(rect);
                }

                GUILayout.FlexibleSpace();
            }

            EditorGUILayout.Space();
        }

        private void ProcessorTypeSelected(Type processorType)
        {
            var processor = (IAssetProcessor)Activator.CreateInstance(processorType);

            AssetProcessorSettings.AddProcessor(processor);
        }

        private static void DrawLine()
        {
            Color color = GUI.color;
            GUI.color = new Color(0.1F, 0.1F, 0.1F);

            Rect rect = EditorGUILayout.GetControlRect(false, 1F);

            GUI.DrawTexture(rect, Texture2D.whiteTexture);
            GUI.color = color;
        }

        private bool DrawMenuButton(GUIContent content, GUIStyle style)
        {
            Rect rect = GUILayoutUtility.GetRect(GUIContent.none, style);

            rect.y += 4F;
            rect.x -= 4F;

            return GUI.Button(rect, content, style);
        }

        private void ShowAssetMenu(string guid)
        {
            var menu = new GenericMenu();

            AssetProcessorSettings.GetProcessors(m_processors);

            for (int i = 0; i < m_processors.Count; i++)
            {
                IAssetProcessor processor = m_processors[i];
                string processorName = processor != null ? ObjectNames.NicifyVariableName(processor.GetType().Name) : "Unknown";

                processorName = $"Add Processor/{processorName}";

                menu.AddItem(new GUIContent(processorName), false, AddProcessorMenu, new object[] { guid, processor });
            }

            menu.AddItem(new GUIContent("Remove"), false, RemoveAssetMenu, guid);

            menu.ShowAsContext();
            m_processors.Clear();
        }

        private void ShowProcessorMenu(int index)
        {
            var menu = new GenericMenu();

            AssetProcessorSettings.GetProcessors(m_processors);

            if (m_processors[index] != null)
            {
                IAssetProcessor processor = m_processors[index];

                menu.AddItem(new GUIContent("Remove"), false, RemoveProcessorMenu, processor);
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Remove"));
            }

            menu.ShowAsContext();
            m_processors.Clear();
        }

        private void ShowAssetProcessorMenu(string guid, int index)
        {
            var menu = new GenericMenu();

            AssetProcessorSettings.GetProcessors(m_processors);

            if (m_processors[index] != null)
            {
                IAssetProcessor processor = m_processors[index];

                menu.AddItem(new GUIContent("Remove"), false, RemoveAssetProcessorMenu, new object[] { guid, processor });
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Remove"));
            }

            menu.ShowAsContext();
            m_processors.Clear();
        }

        private void AddProcessorMenu(object userData)
        {
            var array = (object[])userData;
            string guid = (string)array[0];
            var processor = (IAssetProcessor)array[1];

            AssetProcessorSettings.AddAssetProcessor(guid, processor);
        }

        private void RemoveAssetMenu(object userData)
        {
            string guid = (string)userData;

            AssetProcessorSettings.RemoveAsset(guid);
        }

        private void RemoveProcessorMenu(object userData)
        {
            var processor = (IAssetProcessor)userData;

            AssetProcessorSettings.RemoveProcessor(processor);
        }

        private void RemoveAssetProcessorMenu(object userData)
        {
            var array = (object[])userData;
            string guid = (string)array[0];
            var processor = (IAssetProcessor)array[1];

            AssetProcessorSettings.RemoveAssetProcessor(guid, processor);
        }

        private static string GetProcessorName(SerializedProperty propertyProcessor)
        {
            string processorName = propertyProcessor.managedReferenceFullTypename.Split('.').LastOrDefault();

            if (string.IsNullOrEmpty(processorName))
            {
                processorName = "Unknown";
            }

            return ObjectNames.NicifyVariableName(processorName);
        }
    }
}
