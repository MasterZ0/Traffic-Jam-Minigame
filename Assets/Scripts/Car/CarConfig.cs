using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(CarConfig), fileName = "New" + nameof(CarConfig))]
    public class CarConfig : ScriptableObject
    {
        // TODO: turnAcceleration, reverseSpeed, accelerationRedution
        [Header("Move")]
        [SerializeField] private float accelerationForce = 500f;
        [SerializeField] private Vector2 speedRange = new Vector2(1f, 5f);

        [Header("Brake")]
        [SerializeField] private float brakingForce = 300f;
        [Tooltip("Used to avoid break unnecessarily")]
        [SerializeField] private float brakeThreshold = .2f;

        [Header("Turn")]
        [Range(0f, 50f)]
        [SerializeField] private float maxTurnAngle = 45f;
        [Range(1f, 4f)]
        [SerializeField] private float turnReductionExponent = 1.25f;

        [Tooltip("Below the minimum, no turn value will be equal to " + nameof(maxTurnAngle) + ". Above the maximum, the spin value will be equal to zero")]
        [SerializeField] private Vector2 suspensionForTurnReduction = new Vector2(.5f, .95f);

        [Header("VFX")]
        [SerializeField] private float minSpeedVFX = 2f;

        public float AccelerationForce => accelerationForce;
        public float MinSpeed => speedRange.x;
        public float MaxSpeed => speedRange.y;

        public float BrakingForce => brakingForce;
        public float BrakeThreshold => brakeThreshold;

        public float MaxTurnAngle => maxTurnAngle;
        public float TurnReductionExponent => turnReductionExponent;
        public float MinSuspensionForTurnReduction => suspensionForTurnReduction.x;
        public float MaxSuspensionForTurnReduction => suspensionForTurnReduction.y;

        public float MinSpeedToVFX => minSpeedVFX;
    }
}
