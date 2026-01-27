using System.IO;
using UnityEditor;
using UnityEngine;

namespace App.Editor.Storage
{
    public static class PersistantStorageEditor
    {
        private const string MenuPath = EditorConstants.PersistantStorageToolsPath + "Clear";
        
        [MenuItem(MenuPath, false)]
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