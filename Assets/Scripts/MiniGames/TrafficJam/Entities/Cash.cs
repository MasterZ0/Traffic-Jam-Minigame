using Marmalade.TheGameOfLife.Shared;
using Sirenix.OdinInspector;
using System;
using UnityEngine;
using Z3.ObjectPooling;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class Cash : MonoBehaviour
    {
        [Title("Drop Item")]
        [SerializeField] private Rigidbody rigidbod;
        [SerializeField] private Collider triggerCollider;
        [SerializeField] private Transform shineFX;
        [SerializeField] private TextReference positiveFX;
        [SerializeField] private CashValue cashType;

        public event Action OnCollected;
        public CashValue CashValue => cashType;

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
                if (entity is not ICashHandler player)
                    return;

                triggerCollider.gameObject.SetActive(false);

                int amountToAdd = cashType.GetCashValue();
                player.AddCash(amountToAdd);

                ObjectPool.SpawnPooledObject(shineFX, entity.Center.position, entity.Center.rotation, entity.Center);
                TextReference textReference = ObjectPool.SpawnPooledObject(positiveFX, entity.Center.position, Quaternion.identity);
                textReference.Init(entity.Center, $"+${amountToAdd}");

                OnCollected?.Invoke();
                OnCollected = null;

                this.ReturnToPool();
            }
        }
    }
}
