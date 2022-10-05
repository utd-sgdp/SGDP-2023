// from https://gist.github.com/fguillen/a929a1d003a20bc727d8efe228b5dda4
// which is from https://www.youtube.com/watch?v=6kWUGEQiMUI&ab_channel=whateep
// ReSharper disable StaticMemberInGenericType

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Game.Utility.Patterns
{
    /// <summary>
    /// ScriptableObject Singleton base class that provides lazy-loading from local instances or /Assets/Resources.
    /// Saving must be manually called with <see cref="Save"/>.
    /// </summary>
    /// <typeparam name="T"> The ScriptableObject type deriving from this class. </typeparam>
    public class DynamicSingletonSO<T> : ScriptableObject where T : DynamicSingletonSO<T>
    {
        /// <summary>
        /// Singleton with built-in save functionality. Default data is stored in /Assets/Resources/. Player's data
        /// is stored using <see cref="Application.persistentDataPath"/>.
        /// </summary>
        /// <exception cref="System.Exception"> Unable to find Scriptable Object Singleton. </exception>
        public static T Instance
        {
            get
            {
                if (_instance)
                {
                    return _instance;
                }

                // lazy load _instance from local machine
                _instance = ReadLocalData();
                if (_instance)
                {
                    return _instance;
                }
                
                // lazy load _instance from /Assets/Resources/
                _instance = ReadDefaultData();
                return _instance;
            }
        }
        static T _instance;

        static string s_filePath
        {
            get
            {
                if (_filePath == string.Empty)
                {
                    _filePath = Path.Combine(Application.persistentDataPath, $"{ typeof(T).Name }.json");;
                }

                return _filePath;
            }
        }
        static string _filePath = string.Empty;
        
        /// <summary>
        /// Attempts to read data from local <see cref="DynamicSingletonSO"/> save file.
        /// </summary>
        /// <returns> The object representation of the data. </returns>
        static T ReadLocalData()
        {
            // there is no local data
            if (!File.Exists(s_filePath)) return null;

            // read local file
            string fileContents = File.ReadAllText(s_filePath);

            try
            {
                T obj = CreateInstance<T>();
                JsonUtility.FromJsonOverwrite(fileContents, obj);
                return obj;
            }
            catch (Exception)
            {
                Debug.LogError($"Unable to read SingletonSO ({ typeof(T).Name }) file from { s_filePath }");
                return null;
            }
        }

        /// <summary>
        /// Attempts to read default data from /Assets/Resources/.
        /// </summary>
        /// <returns> Object representation of data. </returns>
        /// <exception cref="Exception"> There is more or less than 1 instance in /Assets/Resources/. </exception>
        static T ReadDefaultData()
        {
            // search resources
            T[] assets = Resources.LoadAll<T>("");
                    
            // validatation
            if(assets == null || assets.Length < 1)
            {
                throw new Exception($"Not found Singleton Scriptable Object of type: { typeof(T).Name }");
            }
            if (assets.Length > 1)
            {
                throw new Exception($"More than 1 instance of Singleton Scriptable Object of type: { typeof(T).Name } found");
            }
                    
            return assets[0];
        }

        /// <summary>
        /// Loads default data. Ignores any local saves.
        /// </summary>
        public static void LoadDefault()
        {
            _instance = ReadDefaultData();
        }

        /// <summary>
        /// Attempts to write <see cref="DynamicSingletonSO"/>'s data to save file.
        /// </summary>
        /// <returns> Success of the operation. </returns>
        [Button(Spacing = 15)]
        public bool Save()
        {
            string jsonString = JsonUtility.ToJson(Instance, true);
            Debug.Log(Instance + " " + jsonString);
            try
            {
                File.WriteAllText(s_filePath, jsonString);
                
                #if UNITY_EDITOR
                    Debug.Log($"Saved { name } to { s_filePath }");
                #endif
                
                return true;
            }
            catch (Exception)
            {
                Debug.LogError($"Unable to write SingletonSO file to { s_filePath }");
                return false;
            }
        }
        
        #if UNITY_EDITOR
            [Button]
            public void ReadLocal()
            {
                Debug.Log($"{name}\n" + ReadLocalData());
            }

            [Button]
            public void ReadDefault()
            {
                Debug.Log($"{name}\n" + ReadDefaultData());
            }
        #endif
    }
}