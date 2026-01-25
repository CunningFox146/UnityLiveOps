using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace Editor.Addressables
{
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
            if (settings == null || settings.ActivePlayerDataBuilder == null)
            {
                Debug.LogWarning("[Addressables] Invalid settings, skipping build.");
                return;
            }

            var builder = settings.ActivePlayerDataBuilder;
            var builderInput = CreateBuilderInput(settings);
            var result = builder.BuildData<AddressablesPlayerBuildResult>(builderInput);

            if (string.IsNullOrEmpty(result.Error))
                return;
            
            Debug.LogError($"[Addressables] Build failed: {result.Error}");
            EditorApplication.isPlaying = false;
        }

        private static AddressablesDataBuilderInput CreateBuilderInput(AddressableAssetSettings settings)
        {
            var builderInput = new AddressablesDataBuilderInput(settings);
            var contentStatePath = ContentUpdateScript.GetContentStateDataPath(false);
            if (string.IsNullOrEmpty(contentStatePath) || !File.Exists(contentStatePath))
                return builderInput;
            
            var previousState = ContentUpdateScript.LoadContentState(contentStatePath);
            if (previousState != null)
            {
                builderInput.PreviousContentState = previousState;
            }

            return builderInput;
        }
    }
}
