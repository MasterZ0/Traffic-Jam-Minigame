using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class PlayerScore : MonoBehaviour
    {
        [SerializeField] private TMP_Text score;
        [SerializeField] private Image background;

        internal void Init(ICashHandler carPawn)
        {
            background.color = carPawn.GetPlayerColor();

            carPawn.OnUpdateCash += OnUpdateScore;
            OnUpdateScore();

            void OnUpdateScore()
            {
                score.text = $"${carPawn.Cash}";
            }
        }
    }
}
