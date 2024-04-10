using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Z3.Effects;
using Z3.ObjectPooling;
using Z3.Utils;
using Z3.Utils.ExtensionMethods;

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

        private TrafficJamConfig config;

        public void Init(TrafficJamConfig config)
        {
            this.config = config;

            timer.TimeInSeconds = config.TimeToSpawnBlackCar.RandomRange();
            timer.OnCompleted += SpawnCar;
        }

        public void UpdateComponent()
        {
            timer.FixedTick();
        }

        private void SpawnCar()
        {
            timer.TimeInSeconds = config.TimeToSpawnBlackCar.RandomRange();
            timer.Reset();

            _ = SpawnWithDelay();
        }

        private async UniTask SpawnWithDelay()
        {
            Transform point = pointsToSpawn.GetRandom();
            spawnEffect.SpawnPooledObject(point.position, point.rotation);

            await UniTask.WaitForSeconds(config.BlackCarSpawnDelay);

            BlackCar carInstance = blackCar.SpawnPooledObject(point.position, point.rotation, carContainer);

            cars.Add(carInstance);
            carInstance.OnFinish += () => cars.Remove(carInstance);
        }

        public void FinishGame()
        {
            foreach (BlackCar car in cars)
            {
                car.GameOver();
            }
        }
    }
}
