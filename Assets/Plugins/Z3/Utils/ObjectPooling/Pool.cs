using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Z3.Utils;

namespace Z3.ObjectPooling
{
    internal class Pool
    {
        internal bool HasAvailableInstances => pool.Count > 0;

        private readonly Transform container;
        private readonly List<GameObject> activeInstances = new List<GameObject>();
        private readonly Queue<GameObject> pool = new Queue<GameObject>();

        /// <summary> This variable allows the same gameobject to have access to more than one component </summary>
        /// <remarks> Key: Instance, Value: Instance components </remarks>
        private static readonly Dictionary<GameObject, CachedComponents> components = new Dictionary<GameObject, CachedComponents>();

        internal const string NullError = "Object Pool Error: Null object sent to pool";
        internal const string ResendError = "Object Pool Error: An object that is already in the pool is being sent again";

        private readonly GameObject original;

        internal Pool(GameObject original, Transform poolContainer)
        {
            this.original = original;
            container = new GameObject($"Pool [{original.name}]").transform;
            container.SetParent(poolContainer);
        }

        internal void AddActiveInstance<T>(T component) where T : Component
        {
            CachedComponents pooledObject = new CachedComponents(component);

            components[component.gameObject] = pooledObject;
            activeInstances.Add(component.gameObject);
        }

        internal void ReturnActiveInstance<T>(T component) where T : Component
        {
            GameObject gameObject = component.gameObject;

            CachedComponents pooledObject = new CachedComponents(component);
            components[gameObject] = pooledObject;

            gameObject.SetActive(false);
            gameObject.transform.SetParent(container);
            pool.Enqueue(gameObject);
        }

        internal T GetFromPool<T>(Vector3 position, Quaternion rotation, Transform parent) where T : Component
        {
            GameObject gameObject = pool.Dequeue();
            activeInstances.Add(gameObject);

            T component = components[gameObject].GetCachedComponent<T>();
            component.transform.SetPositionAndRotation(position, rotation);
            component.transform.SetParent(parent);
            component.transform.localScale = original.transform.localScale;
            component.gameObject.SetActive(original.activeSelf);

            return component;
        }

        internal void ReturnToPool(GameObject gameObject)
        {
            if (!gameObject)
            {
                Debug.LogError(NullError);
                return;
            }

            if (pool.Contains(gameObject))
            {
                Debug.LogError(ResendError);
                return;
            }

            activeInstances.Remove(gameObject);

            gameObject.SetActive(false);
            gameObject.transform.SetParent(container);
            pool.Enqueue(gameObject);
        }

        internal void ReturnAllInstances()
        {
            foreach (GameObject instance in activeInstances.ToList())
            {
                if (!instance)
                {
                    Debug.LogError(NullError);
                    activeInstances.Remove(instance);
                    continue;
                }

                ReturnToPool(instance);
            }
        }
    }
}
