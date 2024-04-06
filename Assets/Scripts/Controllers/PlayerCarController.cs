using Hasbro.TheGameOfLife.Car;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Hasbro.TheGameOfLife.Controllers
{
    public class PlayerCarController : CarTargetFollower
    {
        [Header("Player")]
        [SerializeField] private Transform targetTransform;
        [SerializeField] private Camera mainCamera;

        protected override Vector3 TargetPosition => targetTransform.position;

        private Controls controls;

        protected override void Awake()
        {
            base.Awake();

            controls = new Controls();
            controls.Enable();

            controls.Gameplay.MouseClick.started += SetPosition;
        }

        private void SetPosition(InputAction.CallbackContext context)
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                // Verifica se o raio atingiu algum objeto
                if (hit.collider == null)
                    return;

                targetTransform.position = hit.point;
            }
        }
    }
}