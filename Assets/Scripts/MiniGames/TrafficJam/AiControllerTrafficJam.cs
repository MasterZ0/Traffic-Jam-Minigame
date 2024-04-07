using Marmalade.TheGameOfLife.Car;
using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class AiControllerTrafficJam : CarTargetFollower<TrafficJamCarPawn>
    {
        private Cash targetCash;
        private CashSpawner cashSpawner;

        public void Init(CashSpawner cashSpawner)
        {
            this.cashSpawner = cashSpawner;
            cashSpawner.OnSpawnCash += OnSpawnCash;
        }

        protected override Vector3 GetTargetPosition()
        {
            if (targetCash && targetCash.gameObject.activeSelf)
            {
                return targetCash.transform.position;
            }

            TryFindCash();

            return transform.position;
        }

        private void TryFindCash()
        {
            if (cashSpawner.SpawnedCash.Count == 0)
                return;

            Cash closestCash = null;
            float closestDistance = float.PositiveInfinity;
            foreach (Cash cash in cashSpawner.SpawnedCash)
            {
                float distance = Vector3.Distance(transform.position, cash.transform.position);
                if (distance >= closestDistance)
                    continue;

                closestDistance = distance;
                closestCash = cash;
            }

            targetCash = closestCash;            
        }

        private void OnSpawnCash(Cash cash)
        {
            if (!targetCash)
            {
                targetCash = cash;
                return;
            }

            float currentCashDistance = Vector3.Distance(transform.position, targetCash.transform.position);
            float newCashDistance = Vector3.Distance(transform.position, cash.transform.position);
            if (newCashDistance < currentCashDistance)
            {
                targetCash = cash;
            }
        }
    }
}