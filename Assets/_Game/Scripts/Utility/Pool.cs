// some back-end inspired by Microsoft's object pooling tutorial:
// https://learn.microsoft.com/en-us/dotnet/standard/collections/thread-safe/how-to-create-an-object-pool

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Game.Utility
{
    [CreateAssetMenu(menuName="Object Pool")]
    public class Pool : ScriptableObject
    {
        #region Prefab Pooling
        [SerializeField]
        GameObject _prefab;

        #if UNITY_EDITOR
        [SerializeField, ReadOnly]
        string _sceneName;
        #endif

        Scene _scene => b_scene ??= CreateScene(); 
        Scene? b_scene;
        
        ConcurrentBag<GameObject> _pool => b_pool ??= new ConcurrentBag<GameObject>();
        ConcurrentBag<GameObject> b_pool;

        Scene CreateScene()
        {
            string sceneName = GetSceneName();
            
            #if UNITY_EDITOR
            _sceneName = sceneName;
            #endif
            
            return SceneManager.CreateScene(sceneName);
        }

        string GetSceneName()
        {
            string baseName = $"Pool - { _prefab.name }";
            int i = 0;
            string newName = baseName;
            
            while (SceneManager.GetSceneByName(newName).IsValid())
            {
                newName = baseName + " - " + ++i;
            }

            return newName;
        }

        public void Spawn(TransformData transform)
        {
            GameObject go = CheckOut();
            go.SendMessage("OnSpawn", new PoolAndTransform { Pool = this, TransformData = transform }, SendMessageOptions.DontRequireReceiver);
        }
        
        /// <summary>
        /// Gets instance of <see cref="_prefab"/>. It will be active in the scene, but no
        /// there are no other guarantees about the state of instance.
        /// </summary>
        /// <returns></returns>
        public GameObject CheckOut()
        {
            GameObject item = _pool.TryTake(out GameObject go) ? go : GenerateObject();
            
            item.SetActive(true);
            return item;
        }

        /// <summary>
        /// Return instance of <see cref="_prefab"/> retrieved by <see cref="CheckOut"/>.
        /// </summary>
        /// <param name="go"></param>
        public void CheckIn(GameObject go, bool moveToScene=false)
        {
            go.SetActive(false);
            _pool.Add(go);

            if (moveToScene)
            {
                go.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(go, _scene);
            }
        }

        GameObject GenerateObject()
        {
            GameObject go = Instantiate(_prefab);
            go.SetActive(false);
            SceneManager.MoveGameObjectToScene(go, _scene);
            return go;
        }
        #endregion

        #region Type Pooling
        static Dictionary<Type, PoolData> s_pools;

        public static GameObject CheckOutGameObject()
        {
            PoolData data = GetPoolData<GameObject>();
            ConcurrentBag<GameObject> pool = data.Pool;
            GameObject item = pool.TryTake(out GameObject go) ? go : GenerateGameObject(data.Scene);
            item.SetActive(true);
            return item;
        }

        public static void CheckInGameObject(GameObject go, bool moveToScene=false) => CheckIn<GameObject>(go, moveToScene);

        /// <summary>
        /// Get empty <see cref="GameObject"/> with <typeparamref name="T"/> component
        /// attached.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns>
        /// <see cref="GameObject"/> will be active in scene, but there are no
        /// other guarantees.
        /// </returns>
        public static GameObject CheckOut<T>() where T : Behaviour
        {
            PoolData data = GetPoolData<T>();
            ConcurrentBag<GameObject> pool = data.Pool;
            GameObject item = pool.TryTake(out GameObject go) ? go : GenerateObject<T>(data.Scene);
            item.SetActive(true);
            return item;
        }

        /// <summary>
        /// Return <see cref="GameObject"/> given by <see cref="Pool.CheckOut{T}"/>.
        /// </summary>
        /// <param name="go"></param>
        /// <typeparam name="T"></typeparam>
        public static void CheckIn<T>(GameObject go, bool moveToScene=false)
        {
            // no GameObject was provided :(
            if (!go) return;

            PoolData data = GetPoolData<T>();
            ConcurrentBag<GameObject> pool = data.Pool;
            go.SetActive(false);
            pool.Add(go);
            
            if (moveToScene)
            {
                go.transform.SetParent(null);
                SceneManager.MoveGameObjectToScene(go, data.Scene);
            }
        }

        static PoolData GetPoolData<T>()
        {
            // pool is already present, return that
            Type type = typeof(T);
            if (s_pools != null && s_pools.ContainsKey(type))
            {
                return s_pools[type];
            }

            // make sure its not null
            s_pools ??= new Dictionary<Type, PoolData>();
                
            // make a pool
            PoolData data = new PoolData
            {
                Scene = CreateScene<T>(),
                Pool = new ConcurrentBag<GameObject>(),
            };
            s_pools.Add(type, data);
            
            return data;
        }
        
        static Scene CreateScene<T>()
        {
            string sceneName = GetSceneName<T>();
            return SceneManager.CreateScene(sceneName);
        }
        
        static string GetSceneName<T>()
        {
            string baseName = $"Pool - { typeof(T).Name }";
            int i = 0;
            string newName = baseName;
            
            while (SceneManager.GetSceneByName(newName).IsValid())
            {
                newName = baseName + " - " + ++i;
            }

            return newName;
        }

        static GameObject GenerateObject<T>(Scene scene) where T : Behaviour
        {
            GameObject go = GenerateGameObject(scene, typeof(T).Name);
            go.AddComponent<T>();
            return go;
        }

        static GameObject GenerateGameObject(Scene scene, string name = "Game Object")
        {
            GameObject go = new GameObject(name);
            SceneManager.MoveGameObjectToScene(go, scene);
            return go;
        }

        struct PoolData
        {
            public Scene Scene;
            public ConcurrentBag<GameObject> Pool;
        }
        #endregion
    }

    public struct PoolAndTransform
    {
        public Pool Pool;
        public TransformData TransformData;
    }
}