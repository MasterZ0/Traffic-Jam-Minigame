using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    [Serializable]
    public class CarDebug
    {
        public float currentAcceleration;
        public float currentBreakForce;
        public float currentTurnAngle;
        public bool breaking;
        public float speed;
        public float absSpeed;
    }

    public class CarPawn : MonoBehaviour
    {
        [Header("Car Pawn")]
        [SerializeField] private CarConfig carConfig;

        [Header("Physic Components")]
        [SerializeField] protected Rigidbody carRigidbody;
        [SerializeField] private WheelCollider frontLeftWheel;
        [SerializeField] private WheelCollider frontRightWheel;
        [SerializeField] private WheelCollider backLeftWheel;
        [SerializeField] private WheelCollider backRightWheel;

        [Header("VFX Components")]
        [SerializeField] private ParticleSystem backLeftParticles;
        [SerializeField] private ParticleSystem backRightParticles;
        [SerializeField] private TrailRenderer backLeftTrail;
        [SerializeField] private TrailRenderer backRightTrail;

        [Header("Debug")]
        [ReadOnly, HideLabel, InlineProperty]
        [SerializeField] private CarDebug debug;

        private ICarController controller;
        private List<WheelCollider> wheels;
        private bool breakingVFX;

        public float Speed => Vector3.Dot(carRigidbody.velocity, transform.forward);
        public float AbsSpeed => carRigidbody.velocity.magnitude;
        
        protected virtual void Awake()
        {
            wheels = new List<WheelCollider> 
            { 
                frontLeftWheel, 
                frontRightWheel, 
                backRightWheel, 
                backLeftWheel 
            };
        }

        protected void OnDisable()
        {
            backLeftTrail.Clear();
            backRightTrail.Clear();
        }

        public virtual void Possess(ICarController controller)
        {
            this.controller = controller;
        }

        public virtual void RemoveController() 
        {
            controller = null;
        }

        public void ActiveCar()
        {
            carRigidbody.isKinematic = false;
        }

        public void StopCompletely()
        {
            carRigidbody.isKinematic = true;
            UpdateVFX(false);
        }

        protected virtual void FixedUpdate()
        {
            if (controller == null || !controller.Active)
                return;

            float movement = controller.Movement;
            float direction = controller.Direction;
            bool brake = controller.Brake;

            // Check breaking
            float currentBreakForce = GetBreakForce(brake, movement);
            bool breaking = currentBreakForce != 0;

            float currentAcceleration = GetAcceleration(movement, breaking);
            float currentTurnAngle = GetTurnAngle(direction);

            // Apply values
            frontLeftWheel.steerAngle = currentTurnAngle;
            frontRightWheel.steerAngle = currentTurnAngle;

            foreach (WheelCollider wheel in wheels)
            {
                wheel.motorTorque = currentAcceleration;
                wheel.brakeTorque = currentBreakForce;
            }

            // Update VFX
            UpdateVFX(breaking);

            // Set Debug variables
            debug.currentBreakForce = currentBreakForce;
            debug.currentTurnAngle = currentTurnAngle;
            debug.breaking = breaking;
            debug.currentAcceleration = currentAcceleration;
            debug.speed = Speed;
            debug.absSpeed = AbsSpeed;
        }

        private float GetBreakForce(bool brake, float movement)
        {
            bool breaking = brake || (Speed > carConfig.BrakeThreshold && movement < 0f) || (Speed < -carConfig.BrakeThreshold && movement > 0f);

            if (movement == 0 && AbsSpeed < carConfig.MinSpeed)
            {
                breaking = true;
            }

            return breaking ? carConfig.BrakingForce : 0f;
        }

        private float GetAcceleration(float movement, bool breaking)
        {
            if (breaking)
                return 0f;

            float currentAcceleration = movement * carConfig.AccelerationForce;

            // TODO: Apply reduction by formula
            if (AbsSpeed >= carConfig.MaxSpeed)
            {
                currentAcceleration = 0f;
            }

            return currentAcceleration;
        }

        private float GetTurnAngle(float direction)
        {
            // TODO: Apply Differential steering
            float currentTurnAngle = direction * carConfig.MaxTurnAngle;

            if (direction == 0)
                return currentTurnAngle;

            // Next formula is used to avoid overturn
            WheelCollider wheelToCheck = direction > 0 ? frontRightWheel : frontLeftWheel;

            float suspensionPercentage = GetSuspensionPercentage(wheelToCheck);

            if (suspensionPercentage >= carConfig.MinSuspensionForTurnReduction)
            {
                float normalizedDifference = (suspensionPercentage - carConfig.MinSuspensionForTurnReduction) / (carConfig.MaxSuspensionForTurnReduction - carConfig.MinSuspensionForTurnReduction);
                float exponentialFactor = Mathf.Pow(normalizedDifference, carConfig.TurnReductionExponent);
                currentTurnAngle *= 1.0f - Mathf.Clamp01(exponentialFactor);
            }

            return currentTurnAngle;
        }

        private void UpdateVFX(bool breaking)
        {
            bool breakFxWasActive = breakingVFX;
            breakingVFX = breaking && AbsSpeed > carConfig.MinSpeedToVFX;

            if (breakingVFX == breakFxWasActive)
                return;

            backLeftTrail.emitting = breakingVFX;
            backRightTrail.emitting = breakingVFX;

            if (breakingVFX)
            {
                backLeftParticles.Play();
                backRightParticles.Play();
            }
            else
            {
                backLeftParticles.Stop();
                backRightParticles.Stop();
            }
        }

        /// <remarks> This formula considers that the wheel is positioned in the center of the transform </remarks>
        /// <param name="wheel"></param>
        /// <returns> 0 - 1 </returns>
        public static float GetSuspensionPercentage(WheelCollider wheel)
        {
            wheel.GetWorldPose(out Vector3 position, out _);
            float suspensionDistance = Vector3.Distance(wheel.transform.position, position) * 2f;
            return suspensionDistance / wheel.suspensionDistance;
        }
    }
}