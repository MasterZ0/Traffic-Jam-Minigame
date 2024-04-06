using Hasbro.TheGameOfLife.Car;
using Hasbro.TheGameOfLife.GameSelection;
using Hasbro.TheGameOfLife.Shared;
using System.Collections.Generic;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Game
{
    public struct CarPlayer
    {

    }

    public class TrafficJamController : MonoBehaviour 
    {
        [SerializeField] private List<Transform> carStartingPoints;

        private List<CarPawn> players;

        private void Awake()
        {
            List<Player> players = GameController.Players;
            foreach (Player player in players)
            {
                if (player.PlayerType == PlayerType.Human)
                {

                }
                else if (player.PlayerType == PlayerType.Computer)
                {

                }
            }
        }

        public void SetupPlayers(List<CarPawn> carPawns)
        {
            players = new List<CarPawn>(carPawns);
        }

        public void SetupPlayers()
        {
            for (int i = 0; i < players.Count; i++)
            {
                players[i].SetPosition(carStartingPoints[i].position, carStartingPoints[i].rotation);
            }
        }
    }
}
