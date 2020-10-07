using System;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace UGF.AssetPipeline.Editor
{
    public static class AssetPipelineEditorUtility
    {
        public static string GetAdditionalFilePath(string path, string label, string extensionName)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));
            if (string.IsNullOrEmpty(label)) throw new ArgumentException("Value cannot be null or empty.", nameof(label));
            if (string.IsNullOrEmpty(extensionName)) throw new ArgumentException("Value cannot be null or empty.", nameof(extensionName));

            string directory = Path.GetDirectoryName(path);
            string name = Path.GetFileNameWithoutExtension(path);

            return $"{directory}/{name}.{label}.{extensionName}";
        }

        public static void StartProjectWindowCreateTextFile(string name, string extensionName, string content)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            if (string.IsNullOrEmpty(extensionName)) throw new ArgumentException("Value cannot be null or empty.", nameof(extensionName));
            if (string.IsNullOrEmpty(content)) throw new ArgumentException("Value cannot be null or empty.", nameof(content));

            Texture2D icon = AssetPreview.GetMiniTypeThumbnail(typeof(TextAsset));

            ProjectWindowUtil.CreateAssetWithContent($"{name}.{extensionName}", content, icon);
        }
    }
}
