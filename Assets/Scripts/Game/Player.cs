using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Gameplay
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    public class Player
    {
        public string DisplayName { get; }
        public PlayerType PlayerType { get; }
        public CharacterColor CharacterColor { get; }
        public Color Color { get; }

        public Player(int number, PlayerType playerType, CharacterColor characterColor) : this($"Player {number}", playerType, characterColor)
        {

        }

        public Player(string playerName, PlayerType playerType, CharacterColor characterColor)
        {
            DisplayName = playerName;
            PlayerType = playerType;
            CharacterColor = characterColor;
            Color = characterColor.GetColor();
        }
    }
}
