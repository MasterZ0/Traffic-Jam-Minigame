using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Gameplay;
using Marmalade.TheGameOfLife.Shared;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Z3.ObjectPooling;
using TMPro;
using System;
using Marmalade.TheGameOfLife.ApplicationManager;
using Cysharp.Threading.Tasks;
using Marmalade.TheGameOfLife.Controllers;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    /*
     - Features
    OK - Game with 2 to 4 player
    OK - Each player have you own color
    OK - Player controller by mouse
    OK - Spawn 4 type of moneys, randomly and spontaneously with max money count
    OK - Money collect and update HUD
    OK - Spawn Gray cars and follow a path. 
    OK - Small countdown to start the game (3 seconds)
    OK - 30 segunds of gameplay
    OK - Show winner at the end
    OK - Sprite money FX
    DOING - car controlled by NPC, that follow a target and avoid the traffic by stopping or redirecting
    Include SFX
    Tutorial
    Intro
     */
    public class TrafficJamController : MonoBehaviour
    {
        [Header("Sub Components")]
        [SerializeField] private CashSpawner cashSpawner;
        [SerializeField] private BlackCarSpawner blackCarSpawner;
        [SerializeField] private HUD hud;
        [SerializeField] private ScoreResult scoreResult;

        [Header("Dependencies")]
        [SerializeField] private TMP_Text timer;
        [SerializeField] private TMP_Text middleTitle;
        [SerializeField] private Animator middleTitleAnimator;
        [Space]
        [SerializeField] private GameObject cmGameplay;
        [SerializeField] private GameObject cmScoreResult;
        [Space]
        [SerializeField] private List<Transform> carStartingPoints;

        [Header("Prefabs")]
        [SerializeField] private List<TrafficJamCarPawn> cars;

        [Header("Texts")]
        [SerializeField] private string go = "Go!";
        [SerializeField] private string stop = "Stop";

        [Header("Animation State")]
        [SerializeField] private string startState = "Start";
        [SerializeField] private string countState = "Count";
        [SerializeField] private string stopState = "Stop";

        private readonly List<TrafficJamPlayer> players = new();

        private float time;
        private int secondsToStart;
        private bool timerRunning;

        [Inject]
        private TrafficJamConfig config;

        private void Awake()
        {
            AppManager.WaitLoadingEnd().ContinueWith(() =>
            {
                this.InjectServices();

                cashSpawner.Init(config);
                blackCarSpawner.Init(config);

                SpawnPlayers();
                StartCounter();

            }).Forget();
        }

        private void OnDestroy()
        {
            foreach (TrafficJamPlayer player in players)
            {
                player.Dispose();
            }
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

                CarControllerTargetFollower carController;

                if (player.PlayerType == PlayerType.Human)
                {
                    carController = carPawn.gameObject.AddComponent<PlayerCarController>();
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


                TrafficJamPlayer trafficJamPlayer = new TrafficJamPlayer(player, carController, carPawn);

                players.Add(trafficJamPlayer);
            }

            hud.Init(config, players);
        }

        private void StartCounter()
        {
            time = config.GameDuration;
            UpdateTimer();

            secondsToStart = config.SecondsToStart;
            middleTitle.text = secondsToStart.ToString();

            middleTitleAnimator.Play(countState);
        }

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

        private void UpdateTimer()
        {
            TimeSpan timeSpan = TimeSpan.FromSeconds(time);
            timer.text = $"{timeSpan:ss\\:ff}";
        }

        private void FinishGame()
        {
            foreach (TrafficJamPlayer player in players)
            {
                player.FinishGame(); 
            }

            middleTitleAnimator.Play(stopState);
            middleTitle.text = stop;

            blackCarSpawner.FinishGame();
        }

        #region Title Events
        public void OnCounterTrigger()
        {
            // Update Counter
            secondsToStart--;

            if (secondsToStart > 0)
            {
                middleTitle.text = secondsToStart.ToString();
                return;
            }

            // Start Game
            middleTitle.text = go;
            middleTitleAnimator.Play(startState);

            timerRunning = true;

            foreach (TrafficJamPlayer player in players)
            {
                player.StartGame();
            }
        }

        public void OnStopFinish()
        {
            timer.gameObject.SetActive(false);

            // Change camera
            cmGameplay.SetActive(false);
            cmScoreResult.SetActive(true);

            // Update sub components
            hud.Hide();
            blackCarSpawner.Clear();
            cashSpawner.Clear();

            scoreResult.ShowScore(players);
        }
        #endregion
    }
}
