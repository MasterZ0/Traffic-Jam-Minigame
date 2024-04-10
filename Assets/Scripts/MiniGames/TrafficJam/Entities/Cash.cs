using Marmalade.TheGameOfLife.Shared;
using System;
using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{

    public class Cash : DroppedItem
    {
        [SerializeField] private CashValue cashType;

        public event Action OnCollected;

        public CashValue CashValue => cashType;

        protected override bool TryToCollect(IEntity entity)
        {
            if (entity is not ICashHandler player)
                return false;

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

            return true;
        }
    }
}
