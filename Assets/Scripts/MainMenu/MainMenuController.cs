using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Hasbro.TheGameOfLife.Shared;
using UnityEngine.Events;

namespace Hasbro.TheGameOfLife.GameSelection
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private List<PlayerSelector> players;
        [SerializeField] private UnityEvent onStartGame;

        private void Start()
        {
            foreach (PlayerSelector p in players)
            {
                p.Init();
            }
        }

        public void OnStartGame()
        {
            List<Player> playerInGame = new();

            foreach (PlayerSelector p in players.Where(p => p.PlayerType != PlayerType.None))
            {
                Player player = new Player(p.PlayerType, p.CharacterColor);
                playerInGame.Add(player);
            }

            GameController.SetPlayer(playerInGame);
            onStartGame.Invoke();
        }
    }
}