using UnityEngine;

namespace Hasbro.TheGameOfLife.Car
{
    /// <summary>
    /// Basic implementation of a Car Behaviour
    /// </summary>
    public class CarPawn : MonoBehaviour
    {
        [SerializeField] private Rigidbody carRigidbody;
        [SerializeField] private CarData data;

        public float Speed { get; private set; }
        public float TurnSpeed { get; private set; }

        private CarController controller;

        public void Possess(CarController controller)
        {
            this.controller = controller;
        }

        public void SetPosition(Vector3 position, Quaternion rotation)
        {
            transform.position = position;
            transform.rotation = rotation;
        }

        public void StopCompletely()
        {
            Speed = 0f;
            TurnSpeed = 0f;
        }

        private void Update()
        {
            if (!controller)
                return;

            float forwardAmount = controller.Movement;
            float turnAmount = controller.Direction;

            if (forwardAmount > 0)
            {
                // Accelerating
                Speed += forwardAmount * data.Acceleration * Time.deltaTime;
            }

            if (forwardAmount < 0)
            {
                if (Speed > 0)
                {
                    // Braking
                    Speed += forwardAmount * data.BrakeSpeed * Time.deltaTime;
                }
                else
                {
                    // Reversing
                    Speed += forwardAmount * data.ReverseSpeed * Time.deltaTime;
                }
            }

            if (forwardAmount == 0)
            {
                // Not accelerating or braking
                if (Speed > 0)
                {
                    Speed -= data.IdleSlowdown * Time.deltaTime;
                }

                if (Speed < 0)
                {
                    Speed += data.IdleSlowdown * Time.deltaTime;
                }
            }

            Speed = Mathf.Clamp(Speed, data.SpeedMin, data.SpeedMax);

            carRigidbody.velocity = transform.forward * Speed;

            if (Speed < 0)
            {
                // Going backwards, invert wheels
                turnAmount *= -1f;
            }

            if (turnAmount > 0 || turnAmount < 0)
            {
                // Turning
                if (TurnSpeed > 0 && turnAmount < 0 || TurnSpeed < 0 && turnAmount > 0)
                {
                    // Changing turn direction
                    TurnSpeed = turnAmount * data.MinTurnAmount;
                }

                TurnSpeed += turnAmount * data.TurnSpeedAcceleration * Time.deltaTime;
            }
            else
            {
                // Not turning
                if (TurnSpeed > 0)
                {
                    TurnSpeed -= data.TurnIdleSlowdown * Time.deltaTime;
                }

                if (TurnSpeed < 0)
                {
                    TurnSpeed += data.TurnIdleSlowdown * Time.deltaTime;
                }

                if (TurnSpeed > -1f && TurnSpeed < +1f)
                {
                    // Stop rotating
                    TurnSpeed = 0f;
                }
            }

            float speedNormalized = Speed / data.SpeedMax;
            float invertSpeedNormalized = Mathf.Clamp(1 - speedNormalized, .75f, 1f);

            TurnSpeed = Mathf.Clamp(TurnSpeed, -data.TurnSpeedMax, data.TurnSpeedMax);

            carRigidbody.angularVelocity = new Vector3(0, TurnSpeed * (invertSpeedNormalized * 1f) * Mathf.Deg2Rad, 0);

            if (transform.eulerAngles.x > 2 || transform.eulerAngles.x < -2 || transform.eulerAngles.z > 2 || transform.eulerAngles.z < -2)
            {
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //if (collision.gameObject.layer == GameHandler.SOLID_OBJECTS_LAYER)
            //{
            Speed = Mathf.Clamp(Speed, 0f, 20f);
            //}
        }
    }
}