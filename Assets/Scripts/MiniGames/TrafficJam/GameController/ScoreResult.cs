using Cysharp.Threading.Tasks;
using Marmalade.TheGameOfLife.Gameplay;
using Marmalade.TheGameOfLife.Shared;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnityEngine.ParticleSystem;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class ScoreResult : MonoBehaviour
    {
        [SerializeField] private GameObject playerResultsContainer;
        [SerializeField] private List<PlayerResult> playerResults;
        [SerializeField] private ParticleSystem confettiFX;

        [Header("Dependencies")]
        [SerializeField] private Transform playersReferencePosition;
        [SerializeField] private GameObject winnerContainer;
        [SerializeField] private Image winnerBackground;
        [SerializeField] private Image winnerIcon;
        [SerializeField] private TMP_Text winnerName;
        [SerializeField] private UnityEvent gameOver;
        [SerializeField] private float camOffset = 4; // TEMP

        public float ScoreAnimationDuration => Config.ScoreAnimationDuration;
        public float CamOffset => camOffset;
        public Transform ReferencePosition => playersReferencePosition;
        [Inject]
        public MainCamera MainCamera { get; private set; }
        [Inject]
        public TrafficJamConfig Config { get; private set; }

        internal void ShowScore(List<TrafficJamPlayer> players)
        {
            this.InjectServices();

            // Find winner
            int bestScore = 0;
            TrafficJamPlayer bestPlayer = null; // Null if is draw

            foreach (TrafficJamPlayer player in players)
            {
                if (player.Cash > bestScore)
                {
                    bestScore = player.Cash;
                    bestPlayer = player;
                }
                else if (player.Cash == bestScore)
                {
                    bestPlayer = null;
                }
            }

            // Setup player results
            for (int i = 0; i < playerResults.Count; i++)
            {
                PlayerResult playerResult = playerResults[i];

                if (i < players.Count)
                {
                    TrafficJamPlayer player = players[i];

                    float percentage = bestScore == 0 ? 0 : player.Cash / (float)bestScore;
                    playerResult.Init(this, player, percentage);

                }
                else
                {
                    playerResult.gameObject.SetActive(false);
                }
            }

            // Setup Winner
            bool hasBestPlayer = bestPlayer != null;
            if (hasBestPlayer)
            {
                winnerName.text = bestPlayer.Player.DisplayName;

                Color winnerColor = bestPlayer.Player.Color;
                winnerBackground.color = winnerColor;
                winnerIcon.color = winnerColor;

                MainModule main = confettiFX.main;
                main.startColor = new MinMaxGradient(winnerColor);
            }

            DisplayScore(hasBestPlayer).Forget();
        }

        private async UniTask DisplayScore(bool hasBestPlayer)
        {
            playerResultsContainer.SetActive(true);

            await UniTask.WaitForSeconds(Config.ScoreInitialDelay);

            foreach (PlayerResult result in playerResults)
            {
                result.Play();
            }

            await UniTask.WaitForSeconds(ScoreAnimationDuration);

            if (hasBestPlayer)
            {
                winnerContainer.SetActive(true);
                confettiFX.gameObject.SetActive(true);
            }

            await UniTask.WaitForSeconds(Config.DelayToChangeScene);

            gameOver.Invoke(); // Change scene
        }
    }
}
