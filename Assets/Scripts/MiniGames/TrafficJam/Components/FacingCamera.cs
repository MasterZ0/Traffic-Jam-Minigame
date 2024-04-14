using Marmalade.TheGameOfLife.Gameplay;
using Marmalade.TheGameOfLife.Shared;
using UnityEngine;
using Z3.Utils.ExtensionMethods;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class FacingCamera : MonoBehaviour
    {
        [Inject]
        private MainCamera mainCamera;

        private void OnEnable()
        {
            this.InjectServices();
        }

        private void Update()
        {
            transform.LookAtX(mainCamera.transform.position);
        }
    }
}
