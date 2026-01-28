using System.IO;
using UnityEditor;
using UnityEngine;

namespace App.Editor.Storage
{
    public static class PersistantStorageEditor
    {
        [MenuItem(EditorConstants.PersistantStorageToolsPath + "Copy persistentDataPath", false)]
        private static void CopyPersistentDataPath()
        {
            EditorGUIUtility.systemCopyBuffer = Application.persistentDataPath;
        }
        
        [MenuItem(EditorConstants.PersistantStorageToolsPath + "Clear", false)]
        private static void ClearPersistentStorage()
        {
            Clear(Application.persistentDataPath);
        }
        
        private static void Clear(string path)
        {
            if (!Directory.Exists(path))
                return;
            
            foreach (var file in Directory.GetFiles(path))
            {
                Debug.Log("Deleting " + file);
                File.Delete(file);
            }

            foreach (var directory in Directory.GetDirectories(path))
            {
                Debug.Log("Deleting " + directory);
                Directory.Delete(directory, true);
            }
        }
    }
}