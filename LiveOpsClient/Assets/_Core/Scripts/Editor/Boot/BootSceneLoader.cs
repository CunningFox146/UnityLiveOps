using UnityEditor;
using UnityEditor.SceneManagement;

namespace Editor.Boot
{
    public static class BootSceneLoader
    {
        private const string MenuPath = EditorConstants.BootSceneToolsPath + "Enter Playmode from Boot scene";
        private const string EnabledPrefKey = "BootSceneLoader_Enabled";

        private static bool IsEnabled
        {
            get => EditorPrefs.GetBool(EnabledPrefKey, true);
            set
            {
                EditorPrefs.SetBool(EnabledPrefKey, value);
                UpdatePlayModeStartScene();
            }
        }
        
        static BootSceneLoader()
        {
            EditorApplication.delayCall += UpdatePlayModeStartScene;
        }

        private static void UpdatePlayModeStartScene()
        {
            if (!IsEnabled)
            {
                EditorSceneManager.playModeStartScene = null;
                return;
            }

            var bootScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorConstants.BootScenePath);
            EditorSceneManager.playModeStartScene = bootScene;
        }
        
        [MenuItem(MenuPath, false, 100)]
        private static void Toggle() => IsEnabled = !IsEnabled;

        [MenuItem(MenuPath, true)]
        private static bool ToggleValidate()
        {
            Menu.SetChecked(MenuPath, IsEnabled);
            return true;
        }
        
        [MenuItem(EditorConstants.BootSceneToolsPath + "Show Boot Scene")]
        private static void PingBootScene()
        {
            var bootScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(EditorConstants.BootScenePath);
            EditorGUIUtility.PingObject(bootScene);
            Selection.activeObject = bootScene;
        }
    }
}
