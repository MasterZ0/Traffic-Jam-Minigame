using Hasbro.TheGameOfLife.Shared;
using System;
using UnityEngine;
using Z3.ObjectPooling;

namespace Hasbro.TheGameOfLife.TrafficJam
{
    public class Cash : MonoBehaviour
    {
        [SerializeField] private CashValue cashType;

        public event Action OnCollected;

        public CashValue CashValue => cashType;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.attachedRigidbody || !other.attachedRigidbody.TryGetComponent(out ICashHandler player))
                return;

            int amount = cashType switch
            {
                CashValue.OneThousand => 1000,
                CashValue.FiveThousand => 5000,
                CashValue.TenThousand => 10000,
                CashValue.FiftyThousand => 50000,
                _ => throw new NotImplementedException(),
            };

            player.AddCash(amount);
            
            OnCollected?.Invoke();
            OnCollected = null;

            this.ReturnToPool();
        }
    }
}
