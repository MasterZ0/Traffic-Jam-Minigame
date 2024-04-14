using System.Collections.Generic;
using Marmalade.TheGameOfLife.Shared;

namespace Marmalade.TheGameOfLife.Gameplay
{
    /// <summary>
    /// Core data, used to manage the gameplay
    /// </summary>
    public static class GameController
    {
        private static List<Player> Players { get; set; }

        public static List<Player> GetPlayers() => Players ??= new List<Player>
        {
            new(1, PlayerType.Human, CharacterColor.Blue),
            new(2, PlayerType.Computer, CharacterColor.Pink),
            new(3, PlayerType.Computer, CharacterColor.Orange),
            new(4, PlayerType.Computer, CharacterColor.Yellow),
        };

        public static void SetPlayer(List<Player> players)
        {
            Players = players;
        }
    }
}
