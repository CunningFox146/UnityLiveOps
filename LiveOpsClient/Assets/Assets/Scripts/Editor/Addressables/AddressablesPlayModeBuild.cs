using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Editor
{
    /// <summary>
    /// Automatically builds Addressables before entering Play Mode.
    /// Toggle via: Tools → Addressables → Auto Build On Play
    /// </summary>
    [InitializeOnLoad]
    public static class AddressablesPlayModeBuild
    {
        private const string MenuPath = EditorConstants.AddressablesPath + "Auto Build On Play";
        private const string PrefKey = "Addressables_AutoBuildOnPlay";

        static AddressablesPlayModeBuild()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static bool IsEnabled
        {
            get => EditorPrefs.GetBool(PrefKey, false);
            set => EditorPrefs.SetBool(PrefKey, value);
        }

        [MenuItem(MenuPath, false, 100)]
        private static void Toggle() => IsEnabled = !IsEnabled;

        [MenuItem(MenuPath, true)]
        private static bool ToggleValidate()
        {
            Menu.SetChecked(MenuPath, IsEnabled);
            return true;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (!IsEnabled || state is not PlayModeStateChange.ExitingEditMode)
                return;

            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogWarning("[Addressables] No settings found, skipping build.");
                return;
            }

            Debug.Log("[Addressables] Building...");
            AddressableAssetSettings.BuildPlayerContent(out var result);

            if (!string.IsNullOrEmpty(result.Error))
            {
                Debug.LogError($"[Addressables] Build failed: {result.Error}");
                EditorApplication.isPlaying = false;
                return;
            }

            Debug.Log($"[Addressables] Build complete ({result.Duration:F1}s)");
        }
    }
}
