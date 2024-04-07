﻿using Marmalade.TheGameOfLife.Shared;

namespace Marmalade.TheGameOfLife.Gameplay
{
    /// <summary>
    /// Represents a player in the game.
    /// </summary>
    public class Player
    {
        public PlayerType PlayerType { get; }
        public CharacterColor CharacterColor { get; }

        public Player(PlayerType playerType, CharacterColor characterColor)
        {
            PlayerType = playerType;
            CharacterColor = characterColor;
        }
    }
}