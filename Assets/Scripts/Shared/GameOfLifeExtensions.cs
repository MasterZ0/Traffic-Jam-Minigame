using System;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Shared
{
    public static class GameOfLifeExtensions
    {
        public static Color GetColor(this CharacterColor characterColor)
        {
            return ServiceLocator.GetService<GeneralConfig>().GetColor(characterColor);
        }

        public static int GetCashValue(this CashValue cashValue)
        {
            return cashValue switch
            {
                CashValue.OneThousand => 1000,
                CashValue.FiveThousand => 5000,
                CashValue.TenThousand => 10000,
                CashValue.FiftyThousand => 50000,
                _ => throw new NotImplementedException(),
            };
        }
    }

}
