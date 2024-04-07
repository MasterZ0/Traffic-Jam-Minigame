using UnityEngine;

namespace Z3.ObjectPooling 
{
    public static class ObjectPoolExtensions
    {
        public static GameObject SpawnPooledObject(this GameObject pooledObject, Transform parent = null)
        {
            return ObjectPool.SpawnPooledObject(pooledObject, parent);
        }

        public static GameObject SpawnPooledObject(this GameObject pooledObject, Vector3 position = default, Quaternion rotation = default, Transform parent = null)
        {
            return ObjectPool.SpawnPooledObject(pooledObject, position, rotation, parent);
        }

        public static void ReturnToPool(this GameObject pooledObject)
        {
            ObjectPool.ReturnToPool(pooledObject);
        }

        public static T SpawnPooledObject<T>(this T pooledObject, Transform parent = null) where T : Component
        {
            return ObjectPool.SpawnPooledObject(pooledObject, parent);
        }

        public static T SpawnPooledObject<T>(this T pooledObject, Vector3 position = default, Quaternion rotation = default, Transform parent = null) where T : Component 
        {
            return ObjectPool.SpawnPooledObject(pooledObject, position, rotation, parent);
        }

        public static void ReturnToPool<T>(this T pooledObject) where T : Component 
        {
            ObjectPool.ReturnToPool(pooledObject);
        }
    }
}