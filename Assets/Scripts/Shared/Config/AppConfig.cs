using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Shared
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(AppConfig), fileName = "New" + nameof(AppConfig))]
    public class AppConfig : ScriptableObject, IService
    {
        [SerializeField] private List<ScriptableObject> otherServices;

        [Button("Reload Services"), PropertyOrder(-1)]
        public void Init()
        {
            foreach (IService service in otherServices)
            {
                ServiceLocator.AddService(service);
            }
        }
    }
}
