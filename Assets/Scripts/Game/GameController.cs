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
            new(PlayerType.Human, CharacterColor.Blue),
            new(PlayerType.Computer, CharacterColor.Pink),
            new(PlayerType.Computer, CharacterColor.Orange),
            new(PlayerType.Computer, CharacterColor.Yellow),
        };

        public static void SetPlayer(List<Player> players)
        {
            Players = players;
        }
    }
}
