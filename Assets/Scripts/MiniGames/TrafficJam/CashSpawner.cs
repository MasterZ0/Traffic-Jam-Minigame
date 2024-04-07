using Marmalade.TheGameOfLife.Shared;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Z3.ObjectPooling;
using System;
using Random = UnityEngine.Random;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class CashSpawner : MonoBehaviour
    {
        [SerializeField] private List<Cash> cashNotes;
        [SerializeField] private Transform cashContainer;
        public event Action<Cash> OnSpawnCash;

        private readonly Dictionary<Cash, float> cashProbabilities = new();

        public List<Cash> SpawnedCash { get; private set; } = new();

        [Inject]
        private TrafficJamConfig data;

        private float cashSpawnTimer;

        internal void Init()
        {
            this.InjectServices();

            foreach (Cash cash in cashNotes)
            {
                float weight = cash.CashValue switch
                {
                    CashValue.OneThousand => data.OneThousendChangeWeight,
                    CashValue.FiveThousand => data.FiveThousandChangeWeight,
                    CashValue.TenThousand => data.TenThousandChangeWeight,
                    CashValue.FiftyThousand => data.FiftyThousandChangeWeight,
                    _ => throw new NotImplementedException(),
                };

                cashProbabilities[cash] = weight;
            }
        }

        private void OnDestroy()
        {
            OnSpawnCash = null;
        }

        internal void UpdateComponent()
        {
            // Wait for spaces
            if (SpawnedCash.Count >= data.MaxSpawnedCash)
                return;

            // Update timer
            cashSpawnTimer += Time.fixedDeltaTime;
            if (cashSpawnTimer < data.CashSpawFrequency)
                return;

            cashSpawnTimer -= data.CashSpawFrequency;

            float totalWeight = cashProbabilities.Values.Sum();

            float randomNumber = Random.Range(0f, totalWeight);

            float sum = 0f;
            foreach ((Cash cash, float weight) in cashProbabilities)
            {
                sum += weight;
                if (sum < randomNumber)
                    continue;

                SpawnCash(cash);
                return;
            }

            throw new InvalidOperationException("Could not calculate the cash probability");
        }

        private void SpawnCash(Cash cash)
        {
            //SpawnSmoke smoke = spawSmoke.SpawnPooledObject(position, Quaternion.identity, transform);
            //smoke.Init(OnSpaw, Settings.SmokeWarmingDuration, Settings.SmokeDisappearsDuration, Settings.SmokeDelayToDestroy);

            Vector3 position = GetRandomPosition();
            Cash newCash = ObjectPool.SpawnPooledObject(cash, position, Quaternion.identity, cashContainer);
            
            SpawnedCash.Add(newCash);
            OnSpawnCash?.Invoke(newCash);

            newCash.OnCollected += () =>
            {
                SpawnedCash.Remove(newCash);
            };
        }

        private Vector3 GetRandomPosition()
        {
            int angle = Random.Range(0, 360);
            float area = Random.Range(0, data.SpawRadius);
            return new Vector3()
            {
                x = area * Mathf.Cos(angle * Mathf.Deg2Rad),
                y = 0f,
                z = area * Mathf.Sin(angle * Mathf.Deg2Rad)
            };
        }

        private void OnDrawGizmosSelected()
        {
            if (data == null)
            {
                this.InjectServices();
            }

            Gizmos.DrawWireSphere(transform.position, data.SpawRadius);
        }
    }
}
