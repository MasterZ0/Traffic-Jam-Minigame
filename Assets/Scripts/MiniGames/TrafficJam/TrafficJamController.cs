using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Gameplay;
using Marmalade.TheGameOfLife.Shared;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Z3.ObjectPooling;
using TMPro;
using System;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    /*
     - Features
    OK - Game with 2 to 4 player
    OK - Each player have you own color
    OK - Player controller by mouse
    DOING - car controlled by NPC, that follow a target and avoid the traffic by stopping or redirecting
    OK - Spawn 4 type of moneys, randomly and spontaneously with max money count
    OK - Money collect and update HUD
    DOING - Spawn Gray cars and follow a path. 

    OK - Small countdown to start the game (3 seconds)
    OK - 30 segunds of gameplay
    Show winner at the end
    Include SFX
    Tutorial
    Intro
     */
    public class TrafficJamController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private CashSpawner cashSpawner;
        [SerializeField] private BlackCarSpawner blackCarSpawner;
        [SerializeField] private HUD hud;
        [Space]
        [SerializeField] private List<Transform> carStartingPoints;

        [Header("UI")]
        [SerializeField] private TMP_Text timer;
        [SerializeField] private TMP_Text starterCounter;
        [SerializeField] private Animator startCounterAnimator;

        [Header("Prefabs")]
        [SerializeField] private AiControllerTrafficJam aiController;
        [SerializeField] private PlayerControllerTrafficJam playerController;
        [SerializeField] private List<TrafficJamCarPawn> cars;

        [Header("Texts")]
        [SerializeField] private string go = "Go!";

        [Header("Animation State")]
        [SerializeField] private string startState = "Start";
        [SerializeField] private string countState = "Count";

        private readonly Dictionary<CarController, TrafficJamCarPawn> players = new();

        private float time;
        private int secondsToStart;
        private bool timerRunning;

        [Inject]
        private TrafficJamConfig config;

        private void Awake()
        {
            this.InjectServices();

            cashSpawner.Init(config);
            blackCarSpawner.Init(config);
            SpawnPlayers();
            StartCounter();
        }

        #region Starting
        private void StartCounter()
        {
            time = config.GameDuration;
            UpdateTimer();

            secondsToStart = config.SecondsToStart;
            starterCounter.text = secondsToStart.ToString();

            startCounterAnimator.Play(countState);
        }

        public void OnCounterTrigger()
        {
            secondsToStart--;
            if (secondsToStart > 0)
            {
                starterCounter.text = secondsToStart.ToString();
                return;
            }

            starterCounter.text = go;
            startCounterAnimator.Play(startState);

            timerRunning = true;

            foreach ((CarController c, _) in players)
            {
                c.SetControllerActive(true);
            }
        }
        #endregion

        private void FixedUpdate()
        {
            if (!timerRunning)
                return;

            time -= Time.fixedDeltaTime;

            if (time <= 0f)
            {
                time = 0f;
                timerRunning = false;

                FinishGame();
            }

            cashSpawner.UpdateComponent();
            blackCarSpawner.UpdateComponent();
            UpdateTimer();
        }

        private void FinishGame()
        {
            foreach ((CarController controller, TrafficJamCarPawn car) in players)
            {
                controller.SetControllerActive(false);
                car.StopCompletely();
            }
        }

        private void UpdateTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            timer.text = $"{timeSpan:ss\\:ff}";
        }

        private void SpawnPlayers()
        {
            List<Player> gamePlayers = GameController.GetPlayers();

            for (int i = 0; i < gamePlayers.Count; i++)
            {
                Player player = gamePlayers[i];
                Transform slot = carStartingPoints[i];

                TrafficJamCarPawn carPrefab = cars.First(c => c.CarColor == player.CharacterColor);
                TrafficJamCarPawn carPawn = ObjectPool.SpawnPooledObject(carPrefab, slot.position, slot.rotation, slot);

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
                    throw new NotImplementedException();
                }

                carController.SetControllerActive(false);
                carPawn.SetPlayer(carController, player);
                players.Add(carController, carPawn);
            }

            hud.Init(config, players.Values.ToList());
        }

        private void OnDestroy()
        {
            foreach ((CarController controller, _) in players)
            {
                Destroy(controller);
            }
        }
    }
}
