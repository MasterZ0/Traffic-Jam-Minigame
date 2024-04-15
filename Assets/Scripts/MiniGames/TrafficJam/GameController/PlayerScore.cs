using Cysharp.Threading.Tasks;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class PlayerScore : MonoBehaviour
    {
        [Header("Player Score")]
        [SerializeField] private TMP_Text score;
        [SerializeField] private Image background;
        [SerializeField] private Animator animator;

        [Header("Animation States")]
        [SerializeField] private string defaultState = "Default";
        [SerializeField] private string positiveState = "Positive";
        [SerializeField] private string negativeState = "Negative";

        private TrafficJamPlayer player;
        private int currentValue;
        private bool animating;

        private TrafficJamConfig config;
        private int amountPerTick;

        private readonly CancellationTokenSource cts = new();

        internal void Init(TrafficJamConfig config, TrafficJamPlayer player)
        {
            this.config = config;
            this.player = player;

            player.OnUpdateCash += OnUpdateScore;

            background.color = player.Player.Color;
            currentValue = player.Cash;
            score.text = $"${player.Cash}";
        }

        private void OnDestroy()
        {
            cts.Cancel();
        }

        private async void OnUpdateScore()
        {
            if (currentValue == player.Cash)
                return;

            if (currentValue < player.Cash)
            {
                animator.Play(positiveState);
            }
            else
            {
                animator.Play(negativeState);
            }

            // Calculate amount by percentage
            int difference = Mathf.Abs(player.Cash - currentValue);
            amountPerTick = Mathf.RoundToInt(config.ScoreAmountPerTick * 0.01f * difference);

            // If is already animating, just recalculate update the animation state and amount
            if (animating)
                return;

            animating = true;
            await AnimateScore();

            if (cts.IsCancellationRequested)
                return;

            animator.Play(defaultState);
            animating = false;
        }

        private async UniTask AnimateScore()
        {
            while (player.Cash != currentValue)
            {
                if (currentValue < player.Cash)
                {
                    if (currentValue + amountPerTick > player.Cash)
                        break;

                    currentValue += amountPerTick;
                }
                else
                {
                    if (currentValue - amountPerTick < player.Cash)
                        break;

                    currentValue -= amountPerTick;
                }

                score.text = $"${currentValue}";
                await UniTask.WaitForSeconds(config.ScoreDelayPerTick, cancellationToken: cts.Token).SuppressCancellationThrow();
            }

            currentValue = player.Cash;
            score.text = $"${currentValue}";
        }
    }
}
