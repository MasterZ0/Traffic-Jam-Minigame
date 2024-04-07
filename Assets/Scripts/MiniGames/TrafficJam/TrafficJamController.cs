using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Gameplay;
using Marmalade.TheGameOfLife.Shared;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Z3.ObjectPooling;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class TrafficJamController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CashSpawner cashSpawner;
        [SerializeField] private HUD hud;
        [Space]
        [SerializeField] private List<Transform> carStartingPoints;

        [Header("Prefabs")]
        [SerializeField] private List<TrafficJamCarPawn> cars;


        private Dictionary<CarController, TrafficJamCarPawn> players = new();

        [Inject]
        private TrafficJamConfig data;

        private void Awake()
        {
            this.InjectServices();

            cashSpawner.Init();
            SpawnPlayers();
        }

        private void FixedUpdate()
        {
            cashSpawner.UpdateComponent();
        }

        private void SpawnPlayers()
        {
            List<Player> gamePlayers = GameController.GetPlayers();

            for (int i = 0; i < gamePlayers.Count; i++)
            {
                Player player = gamePlayers[i];

                TrafficJamCarPawn carPawn = SpawnCar(player, carStartingPoints[i]);
                CarController<TrafficJamCarPawn> carController;

                if (player.PlayerType == PlayerType.Human)
                {
                    carController = carPawn.gameObject.AddComponent<PlayerControllerTrafficJam>();
                }
                else if (player.PlayerType == PlayerType.Computer)
                {
                    AiControllerTrafficJam carAi = carPawn.gameObject.AddComponent<AiControllerTrafficJam>();
                    carAi.Init(cashSpawner);
                    carController = carAi;
                }
                else
                {
                    throw new System.NotImplementedException();
                }

                carPawn.SetPlayer(carController, player);
                players.Add(carController, carPawn);
            }

            hud.Init(players.Values.ToList());
        }

        private TrafficJamCarPawn SpawnCar(Player player, Transform slot)
        {
            TrafficJamCarPawn carPrefab = cars.First(c => c.CarColor == player.CharacterColor);
            TrafficJamCarPawn newCarPawn = ObjectPool.SpawnPooledObject(carPrefab, slot.position, slot.rotation, slot);


            return newCarPawn;
        }

        private void OnDestroy()
        {
            foreach ((CarController c, _) in players)
            {
                Destroy(c);
            }
        }
    }
}
