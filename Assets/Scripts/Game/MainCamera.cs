using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Gameplay
{
    public class MainCamera : MonoBehaviour, IService
    {
        [SerializeField] private Camera mainCamara;

        public Camera Camera => mainCamara;

        private void Awake()
        {
            ServiceLocator.AddService(this);
        }

        private void OnDestroy()
        {
            ServiceLocator.RemoveService(this);
        }
    }
}
