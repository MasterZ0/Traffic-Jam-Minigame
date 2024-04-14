using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Gameplay;
using Marmalade.TheGameOfLife.Shared;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Marmalade.TheGameOfLife.Controllers
{
    public class PlayerCarController : CarControllerTargetFollower
    {
        [Inject]
        private MainCamera mainCamera;

        private Controls controls;
        private Vector3 targetPosition;
        private bool pressed;

        protected override void Awake()
        {
            base.Awake();

            controls = new Controls();
            controls.Gameplay.MouseClick.started += OnPresses;
            controls.Gameplay.MouseClick.canceled += OnRelease;

            controls.Enable();
        }

        protected void OnDestroy()
        {
            controls.Dispose();
        }

        public override void SetControllerActive(bool active)
        {
            base.SetControllerActive(active);

            if (active)
            {
                controls.Enable();
            }
            else
            {
                controls.Disable();
            }
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            targetPosition = transform.position;
        }

        private void OnPresses(InputAction.CallbackContext context)
        {
            pressed = true;
            Pawn.ActiveTargetAnimator(true);
        }

        private void OnRelease(InputAction.CallbackContext context)
        {
            pressed = false;
            Pawn.ActiveTargetAnimator(false);
        }

        protected override Vector3 GetTargetPosition() => targetPosition;

        protected override void FixedUpdate()
        {
            UpdateTargetPosition();
            base.FixedUpdate();

            if (ReachedTarget && !pressed)
            {
                targetPosition = transform.position;
            }
        }

        private void UpdateTargetPosition()
        {
            if (!pressed)
                return;

            Vector2 mousePosition = controls.Gameplay.MousePosition.ReadValue<Vector2>();
            Ray ray = mainCamera.Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, float.PositiveInfinity, Pawn.ScenaryLayer))
            {
                if (hit.collider == null)
                    return;

                targetPosition = hit.point;
            }

            Pawn.SetTargetPosition(targetPosition);
        }
    }
}