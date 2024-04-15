using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Z3.Effects;
using Z3.ObjectPooling;
using Z3.Utils.ExtensionMethods;
using Timer = Z3.Utils.Timer;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class BlackCarSpawner : MonoBehaviour
    {
        [Header("Black Car Spawner")]
        [SerializeField] private Transform carContainer;
        [SerializeField] private AnimationVFX spawnEffect;
        [SerializeField] private List<Transform> pointsToSpawn;

        [Header("Prefabs")]
        [SerializeField] private BlackCar blackCar;

        private readonly List<BlackCar> cars = new();
        private readonly Timer timer = new();

        private CancellationTokenSource cts = new();
        private TrafficJamConfig config;

        internal void Init(TrafficJamConfig config)
        {
            this.config = config;

            timer.TimeInSeconds = config.TimeToSpawnBlackCar.RandomRange();
            timer.OnCompleted += SpawnCar;
        }

        private void OnDestroy()
        {
            cts.Cancel();
            timer.Dispose();
        }

        internal void UpdateComponent()
        {
            timer.FixedTick();
        }

        internal void FinishGame()
        {
            cts.Cancel();

            foreach (BlackCar car in cars)
            {
                car.GameOver();
            }
        }

        internal void Clear()
        {
            foreach (BlackCar car in cars)
            {
                car.ReturnToPool();
            }
        }

        private void SpawnCar()
        {
            timer.TimeInSeconds = config.TimeToSpawnBlackCar.RandomRange();
            timer.Reset();

            SpawnWithDelay().Forget();
        }

        private async UniTask SpawnWithDelay()
        {
            Transform point = pointsToSpawn.GetRandom();
            spawnEffect.SpawnPooledObject(point.position, point.rotation);

            await UniTask.WaitForSeconds(config.BlackCarSpawnDelay, cancellationToken: cts.Token);

            BlackCar carInstance = blackCar.SpawnPooledObject(point.position, point.rotation, carContainer);

            cars.Add(carInstance);
            carInstance.OnFinish += () => cars.Remove(carInstance);
        }
    }
}
