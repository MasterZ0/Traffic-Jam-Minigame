using UnityEngine;

namespace Hasbro.TheGameOfLife.Shared
{
    public static class GameOfLifeExtensions
    {
        public static Color GetColor(this CharacterColor characterColor)
        {
            return ServiceLocator.GetService<GeneralConfig>().GetColor(characterColor);
        }
    }

}
