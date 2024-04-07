using UnityEngine;

namespace Z3.ObjectPooling
{
    public class ObjectPoolContainer : MonoBehaviour
    {
        public static Transform SpawnContainer => Instance.transform;

        private static ObjectPoolContainer instance;
        private static ObjectPoolContainer Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject pool = new GameObject("ObjectPoolContainer [Generated]");
                    ObjectPoolContainer objectPool = pool.AddComponent<ObjectPoolContainer>();
                    instance = objectPool;
                }
                
                return instance;
            }
            set => instance = value;
        }

        private void Awake()
        {
            Instance = this;
        }

        private void OnDestroy()
        {
            foreach (Transform item in transform)
            {
                item.ReturnToPool();
            }
        }
    }
}