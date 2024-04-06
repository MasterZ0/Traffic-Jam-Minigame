using UnityEngine;
using System;
using System.Collections.Generic;
using Hasbro.TheGameOfLife.Shared;

namespace Hasbro.TheGameOfLife.GameSelection
{
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

    public static class GameController
    {
        public static List<Player> Players { get; private set; }

        public static void SetPlayer(List<Player> players)
        {
            Players = players;
        }

        public static Color GetColor(CharacterColor characterType)
        {
            return characterType switch
            {
                CharacterColor.Blue => new Color(0.3137255f, 0.6901961f, 0.9725491f),
                CharacterColor.Orange => new Color(1, 0.5843138f, 0.2509804f),
                CharacterColor.Yellow => new Color(0.9019608f, 1, 0.2509804f),
                CharacterColor.Pink => new Color(1, 0.4745098f, 1),
                _ => throw new NotImplementedException(),
            };
        }
    }
}
