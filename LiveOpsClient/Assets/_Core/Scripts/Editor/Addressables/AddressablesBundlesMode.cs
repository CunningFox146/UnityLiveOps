using System.IO;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Build;
using UnityEditor.AddressableAssets.Build.DataBuilders;
using UnityEditor.AddressableAssets.Settings;
using UnityEngine;

namespace App.Editor.Addressables
{
    [InitializeOnLoad]
    public static class AddressablesBundlesMode
    {
        private const string MenuPath = EditorConstants.AddressablesPath + "Use Bundles Mode";
        private const string PrefKey = "Addressables_UseBundleMode";

        static AddressablesBundlesMode()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        private static bool IsEnabled
        {
            get => EditorPrefs.GetBool(PrefKey, false);
            set => EditorPrefs.SetBool(PrefKey, value);
        }

        [MenuItem(MenuPath, false, 100)]
        private static void Toggle()
        {
            IsEnabled = !IsEnabled;
            TogglePlayMode();
        }

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
        
        private static void TogglePlayMode()
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;
            if (settings == null)
            {
                Debug.LogWarning("[Addressables] No settings found.");
                return;
            }

            if (IsEnabled)
                SetPlayMode<BuildScriptPackedPlayMode>(settings);
            else
                SetPlayMode<BuildScriptFastMode>(settings);
        }
        
        private static void SetPlayMode<T>(AddressableAssetSettings settings) where T : IDataBuilder
        {
            for (var i = 0; i < settings.DataBuilders.Count; i++)
            {
                if (settings.DataBuilders[i] is not T)
                    continue;
                
                settings.ActivePlayModeDataBuilderIndex = i;
                EditorUtility.SetDirty(settings);
                return;
            }

            Debug.LogWarning($"[Addressables] Could not find {typeof(T).Name} in DataBuilders.");
        }
    }
}
