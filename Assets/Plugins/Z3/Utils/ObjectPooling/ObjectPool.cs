using System.Collections.Generic;
using UnityEngine;

namespace Z3.ObjectPooling 
{
    /// <summary>
    /// Manages game resources for processing gain (better garbage collection usage)
    /// </summary>
    public class ObjectPool : MonoBehaviour
    {
        /// <summary> Key can be Prefabs and Instances </summary>
        private static readonly Dictionary<GameObject, Pool> pools = new Dictionary<GameObject, Pool>();

        private static Transform poolContainer;
        private static Transform PoolContainer
        {
            get
            {
                if (!poolContainer)
                {
                    GameObject pool = new GameObject("ObjectPool [Generated]");
                    poolContainer = pool.AddComponent<ObjectPool>().transform;
                }

                return poolContainer;
            }
        }

        private void Awake()
        {
            if (!poolContainer)
            {
                poolContainer = transform;
                return;
            }

            if (poolContainer == this)
                return;
            
            Debug.LogError($"There is already a Object Pool instance '{poolContainer.name}'", gameObject);
            Destroy(gameObject);
        }

        public static GameObject SpawnPooledObject(GameObject prefab, Transform parent) 
        {
            return SpawnPooledObject(prefab, default, default, parent);
        }

        public static GameObject SpawnPooledObject(GameObject prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            return SpawnPooledObject(prefab.transform, position, rotation, parent).gameObject;
        }

        public static T SpawnPooledObject<T>(T prefab, Transform parent) where T : Component
        {
            return SpawnPooledObject(prefab, default, default, parent);
        }

        public static T SpawnPooledObject<T>(T prefab, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Component
        {
            if (!parent)
            {
                parent = ObjectPoolContainer.SpawnContainer;
            }

            // Try to get the pool for the prefab
            if (!pools.TryGetValue(prefab.gameObject, out Pool pool))
            {
                // Create a new pool for the prefab
                pool = new Pool(prefab.gameObject, PoolContainer);
                pools[prefab.gameObject] = pool;
            }
            else if (pool.HasAvailableInstances)
            {
                // Try get an instance from the pool
                return pool.GetFromPool<T>(position, rotation, parent);
            }

            // Instantiate a new instance of the prefab
            T newComponent = Instantiate(prefab, position, rotation, parent);

            // Setup the new instance
            pool.AddActiveInstance(newComponent);
            pools[newComponent.gameObject] = pool;

            return newComponent;
        }

        public static void ReturnToPool(GameObject instance) => ReturnToPool(instance.transform);

        public static void ReturnToPool<T>(T instance) where T : Component
        {
            // Exist pool?
            if (pools.TryGetValue(instance.gameObject, out Pool pool))
            {
                pool.ReturnToPool(instance.gameObject);
                return;
            }

            // Create new Pool
            Pool newPool = new Pool(instance.gameObject, PoolContainer);

            // Setup the new instance
            pools[instance.gameObject] = newPool;
            newPool.ReturnActiveInstance(instance);
            newPool.ReturnToPool(instance.gameObject);
        }

        public static void ReturnAllToPool()
        {
            foreach (Pool pool in pools.Values)
            {
                pool.ReturnAllInstances();
            }
        }

        private void OnDestroy()
        {
            pools.Clear();
        }
    }
}