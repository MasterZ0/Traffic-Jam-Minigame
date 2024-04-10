using Cysharp.Threading.Tasks;
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

        private ICashHandler playerCash;
        private int currentValue;
        private bool animating;

        private TrafficJamConfig config;
        private int amountPerTick;

        internal void Init(TrafficJamConfig config, ICashHandler playerCash)
        {
            this.config = config;
            this.playerCash = playerCash;

            playerCash.OnUpdateCash += OnUpdateScore;

            background.color = playerCash.GetPlayerColor();
            currentValue = playerCash.Cash;
            score.text = $"${playerCash.Cash}";
        }

        private async void OnUpdateScore()
        {
            if (currentValue == playerCash.Cash)
                return;

            if (currentValue < playerCash.Cash)
            {
                animator.Play(positiveState);
            }
            else
            {
                animator.Play(negativeState);
            }

            int difference = Mathf.Abs(playerCash.Cash - currentValue);
            amountPerTick = Mathf.RoundToInt(config.ScoreAmountPerTick * 0.01f * difference);

            if (animating)
                return;

            animating = true;
            await AnimateScore();

            animator.Play(defaultState);
            animating = false;
        }

        private async UniTask AnimateScore()
        {
            while (playerCash.Cash != currentValue)
            {
                if (currentValue < playerCash.Cash)
                {
                    if (currentValue + amountPerTick > playerCash.Cash)
                        break;

                    currentValue += amountPerTick;
                }
                else
                {
                    if (currentValue - amountPerTick < playerCash.Cash)
                        break;

                    currentValue -= amountPerTick;
                }

                score.text = $"${currentValue}";
                await UniTask.WaitForSeconds(config.ScoreDelayPerTick);
            }

            currentValue = playerCash.Cash;
            score.text = $"${currentValue}";
        }
    }
}
