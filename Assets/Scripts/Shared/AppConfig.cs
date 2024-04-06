using System;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Shared
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(AppConfig), fileName = "New" + nameof(AppConfig))]
    public class AppConfig : ScriptableObject
    {
        public void Init()
        {
            ServiceLocator.AddService(this);
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
