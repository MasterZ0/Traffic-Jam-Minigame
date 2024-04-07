using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Hasbro.TheGameOfLife.Shared;
using UnityEngine.Events;
using Hasbro.TheGameOfLife.Gameplay;

namespace Hasbro.TheGameOfLife.MainMenu
{
    public class MainMenuController : MonoBehaviour
    {
        [SerializeField] private List<PlayerSelector> players;
        [SerializeField] private UnityEvent onStartGame;

        private void Start()
        {
            GeneralConfig config = ServiceLocator.GetService<GeneralConfig>();

            foreach (PlayerSelector p in players)
            {
                p.Init(config);
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