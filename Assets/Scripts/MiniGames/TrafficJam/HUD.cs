using System.Collections.Generic;
using UnityEngine;

namespace Hasbro.TheGameOfLife.TrafficJam
{
    public class HUD : MonoBehaviour
    {
        [SerializeField] private GameObject pauseScreen;
        [SerializeField] private List<PlayerScore> playerScore;

        public void Init(List<TrafficJamCarPawn> players)
        {
            for (int i = 0; i < playerScore.Count; i++)
            {
                if (i < players.Count)
                {
                    playerScore[i].Init(players[i]);
                }
                else
                {
                    playerScore[i].gameObject.SetActive(false);
                }
            }
        }

        public void OnPause()
        {
            Time.timeScale = 0f;
            pauseScreen.SetActive(true);
        }

        public void OnResume()
        {
            Time.timeScale = 1f;
            pauseScreen.SetActive(false);
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }
    }
}
