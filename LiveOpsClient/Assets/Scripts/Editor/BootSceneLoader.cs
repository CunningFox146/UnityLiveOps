using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Editor
{
    [InitializeOnLoad]
    public static class BootSceneLoader
    {
        private const string BootScenePath = "Assets/Scenes/Boot.unity";
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

            var bootScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootScenePath);
            EditorSceneManager.playModeStartScene = bootScene;
        }
        
        [MenuItem("Tools/Boot Scene Loader/Enable")]
        private static void Enable() => IsEnabled = true;
        [MenuItem("Tools/Boot Scene Loader/Disable")]
        private static void Disable() => IsEnabled = false;

        [MenuItem("Tools/Boot Scene Loader/Enable", true)]
        private static bool EnableValidate() => !IsEnabled;
        [MenuItem("Tools/Boot Scene Loader/Disable", true)]
        private static bool DisableValidate() => IsEnabled;
        
        [MenuItem("Tools/Boot Scene Loader/Show Boot Scene")]
        private static void PingBootScene()
        {
            var bootScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(BootScenePath);
            EditorGUIUtility.PingObject(bootScene);
            Selection.activeObject = bootScene;
        }
    }
}
