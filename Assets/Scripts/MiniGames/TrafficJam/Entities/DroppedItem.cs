using Marmalade.TheGameOfLife.Shared;
using Sirenix.OdinInspector;
using UnityEngine;
using Z3.ObjectPooling;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class DroppedItem : MonoBehaviour
    {
        [Title("Drop Item")]
        [SerializeField] private Rigidbody rigidbod;
        [SerializeField] private Collider triggerCollider;
        [SerializeField] private Transform collectFX;

        [Inject]
        private GeneralConfig Config { get; set; }

        private void Awake()
        {
            this.InjectServices();
        }

        protected virtual void OnEnable()
        {
            triggerCollider.gameObject.SetActive(true);
        }

        private void OnTriggerEnter(Collider col)
        {
            if (col.attachedRigidbody && col.attachedRigidbody.TryGetComponent(out IEntity entity))
            {
                bool tryCollect = TryToCollect(entity);

                if (!tryCollect)
                    return;

                triggerCollider.gameObject.SetActive(false);
                ObjectPool.SpawnPooledObject(collectFX, entity.Center.position, entity.Center.rotation, entity.Center);
                this.ReturnToPool();
            }
        }

        protected virtual bool TryToCollect(IEntity attachedRigidbody) => false;
    }
}
