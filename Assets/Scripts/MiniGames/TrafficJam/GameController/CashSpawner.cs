using Marmalade.TheGameOfLife.Shared;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Z3.ObjectPooling;
using Z3.Utils;
using System;
using Random = UnityEngine.Random;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class CashSpawner : MonoBehaviour
    {
        [Header("Cash Spawner")]
        [SerializeField] private LayerMask forbiddenLayer;
        [SerializeField] private Transform cashContainer;
        [SerializeField] private Transform spawnFX;
        [SerializeField] private List<Cash> cashNotes;

        public event Action<Cash> OnSpawnCash;
        public List<Cash> SpawnedCash { get; private set; } = new();

        private TrafficJamConfig data;

        private readonly Dictionary<Cash, float> cashProbabilities = new();
        private readonly Timer timer = new();

        internal void Init(TrafficJamConfig config)
        {
            data = config;

            this.InjectServices();

            timer.TimeInSeconds = config.CashSpawFrequency;
            timer.OnCompleted += SpawnCash;

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
            timer.Dispose();
            OnSpawnCash = null;
        }

        internal void UpdateComponent()
        {
            // Wait for spaces
            if (SpawnedCash.Count >= data.MaxSpawnedCash)
                return;

            timer.FixedTick();
        }

        internal void Clear()
        {
            foreach (Cash cash in SpawnedCash)
            {
                cash.ReturnToPool();
            }
        }

        private void SpawnCash()
        {
            timer.Reset();

            Cash cash = GetRandomCash();
            Vector3 position = GetRandomPosition();

            Cash newCash = ObjectPool.SpawnPooledObject(cash, position, Quaternion.identity, cashContainer);
            ObjectPool.SpawnPooledObject(spawnFX, position, Quaternion.identity, cashContainer);

            SpawnedCash.Add(newCash);
            OnSpawnCash?.Invoke(newCash);

            newCash.OnCollected += () =>
            {
                SpawnedCash.Remove(newCash);
            };
        }

        private Cash GetRandomCash()
        {
            float totalWeight = cashProbabilities.Values.Sum();

            float randomNumber = Random.Range(0f, totalWeight);

            float sum = 0f;
            foreach ((Cash cash, float weight) in cashProbabilities)
            {
                sum += weight;
                if (sum < randomNumber)
                    continue;

                return cash;
            }

            throw new InvalidOperationException("Could not calculate the cash probability");
        }

        private Vector3 GetRandomPosition()
        {
            const int MaxAttempts = 100;
            Vector3 position;

            for (int attempts = 0; attempts < MaxAttempts; attempts++)
            {
                int angle = Random.Range(0, 360);
                float area = Random.Range(0, data.ArenaRadius);
                position = new Vector3()
                {
                    x = area * Mathf.Cos(angle * Mathf.Deg2Rad),
                    y = 0f,
                    z = area * Mathf.Sin(angle * Mathf.Deg2Rad)
                };

                // Check if there is a player close
                if (!Physics.CheckSphere(position, data.AreaToCheckPlayer, forbiddenLayer))
                {
                    return position;
                }
            }

            Debug.LogError("Maximum number of attempts reached. Unable to find a suitable position.");
            return default;
        }

        private void OnDrawGizmosSelected()
        {
            if (data == null)
            {
                data = AppConfig.GetData<TrafficJamConfig>();
                return;
            }

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, data.ArenaRadius);

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, data.AreaToCheckPlayer);
        }
    }
}
