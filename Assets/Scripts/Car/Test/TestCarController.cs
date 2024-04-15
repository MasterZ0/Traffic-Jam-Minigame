using Marmalade.TheGameOfLife.Controllers;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    public class TestCarController : CarController
    {
        private Controls controls;

        protected override void Awake()
        {
            base.Awake();

            controls = new Controls();
            controls.Enable();
        }

        private void FixedUpdate()
        {
            Vector2 movement = controls.Gameplay.Move.ReadValue<Vector2>();
            movement.x = Mathf.Round(movement.x);
            movement.y = Mathf.Round(movement.y);

            Movement = movement.y;
            Direction = movement.x;
        }
    }
}
