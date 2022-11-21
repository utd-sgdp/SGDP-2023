#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace GameEditor.Utility
{
    [InitializeOnLoad]
    public static class EmptyFolderProtection
    {
        static EmptyFolderProtection()
        {
            EditorApplication.projectChanged += OnProjectChange;
        }
        
        static void OnProjectChange() => UpdateEmptyDirs();
        static void UpdateEmptyDirs()
        {
            // look through all sub-directories of /Assets/
            Stack<string> paths = new Stack<string>();
            paths.Push(Application.dataPath);
            
            while (paths.Count > 0)
            {
                string dir = paths.Pop();
                
                // get sub-directories
                IEnumerable<string> subDirs = Directory.GetDirectories(dir);
                foreach (var subDir in subDirs) paths.Push(subDir);
                
                // get sub-files
                string[] subFiles = Directory.GetFiles(dir, "*");

                bool hasSubDir  = subDirs.Any();
                bool hasSubFile = subFiles.Length != 0;
                string emptydir = Path.Combine(dir, ".emptydir");
                
                // case: no sub-files or sub-directories
                // add .emptydir file for git to track this folder
                if (!hasSubDir && !hasSubFile)
                {
                    using (var fs = File.Create(emptydir)) { }
                    continue;
                }

                // case: there is no extra emptydir to remove
                bool hasEmptydir = File.Exists(emptydir);
                bool hasOnlyEmptydir = !hasSubDir && subFiles.Length == 1;
                if (hasOnlyEmptydir || !hasEmptydir)
                {
                    continue;
                }
                
                File.Delete(emptydir);
            }
        }
    }
}
#endif