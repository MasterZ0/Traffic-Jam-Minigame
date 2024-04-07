using Hasbro.TheGameOfLife.Car;
using Hasbro.TheGameOfLife.Gameplay;
using Hasbro.TheGameOfLife.Shared;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Controllers
{
    public abstract class PlayerCarController<TCarPawn> : CarTargetFollower<TCarPawn> where TCarPawn : CarPawn
    {
        [Inject]
        private MainCamera mainCamera;

        private Controls controls;
        private Vector3 targetPosition;
        private bool follow;

        protected override void Awake()
        {
            base.Awake();

            controls = new Controls();
            controls.Gameplay.MouseClick.started += _ => follow = true;
            controls.Gameplay.MouseClick.canceled += _ => follow = false;
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            targetPosition = transform.position;
            controls.Enable();
        }

        protected void OnDisable()
        {
            controls.Disable();
        }

        protected override void FixedUpdate()
        {
            UpdateTargetPosition();
            base.FixedUpdate();
        }

        protected override Vector3 GetTargetPosition() => targetPosition;

        private void UpdateTargetPosition()
        {
            if (!follow)
                return;

            Vector2 mousePosition = controls.Gameplay.MousePosition.ReadValue<Vector2>();
            Ray ray = mainCamera.Camera.ScreenPointToRay(mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider == null)
                    return;

                targetPosition = hit.point;
            }
        }
    }
}