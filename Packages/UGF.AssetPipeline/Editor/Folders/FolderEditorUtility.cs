using System;
using System.IO;
using UnityEditor;

namespace UGF.AssetPipeline.Editor.Folders
{
    public static class FolderEditorUtility
    {
        private static readonly char[] m_trimIgnoreMark = { '~' };

        public static bool IsIncluded(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));

            return AssetDatabase.IsValidFolder(path);
        }

        public static void Include(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));

            path = path.TrimEnd(m_trimIgnoreMark);

            Directory.Move($"{path}~", path);
        }

        public static void Exclude(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentException("Value cannot be null or empty.", nameof(path));

            path = path.TrimEnd(m_trimIgnoreMark);

            Directory.Move(path, $"{path}~");
        }
    }
}
