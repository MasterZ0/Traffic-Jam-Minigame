﻿using System;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Shared
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(GeneralConfig), fileName = "New" + nameof(GeneralConfig))]
    public class GeneralConfig: ScriptableObject, IService
    {
        [SerializeField] private Color blueCharacter = new Color(0.314f, 0.690f, 0.973f);
        [SerializeField] private Color pinkCharacter = new Color(1, 0.475f, 1);
        [SerializeField] private Color orangeCharacter = new Color(1, 0.584f, 0.2514f);
        [SerializeField] private Color yellowCharacter = new Color(0.766f, 0.908f, 0.251f);

        public Color GetColor(CharacterColor characterType)
        {
            return characterType switch
            {
                CharacterColor.Blue => blueCharacter,
                CharacterColor.Pink => pinkCharacter,
                CharacterColor.Orange => orangeCharacter,
                CharacterColor.Yellow => yellowCharacter,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
