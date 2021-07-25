using UGF.AssetPipeline.Editor.Folders;
using UnityEditor;

namespace UGF.AssetPipeline.Editor.Tests.Folders
{
    public static class TestFolderEditorUtilityMenu
    {
        [MenuItem("Tests/Folders/Include", false, 2000)]
        private static void IncludeMenu()
        {
            FolderEditorUtility.Include("Assets/UGF.AssetPipeline.Editor.Tests/Folders/Test");
        }

        [MenuItem("Tests/Folders/Exclude", false, 2000)]
        private static void ExcludeMenu()
        {
            FolderEditorUtility.Exclude("Assets/UGF.AssetPipeline.Editor.Tests/Folders/Test");
        }
    }
}
