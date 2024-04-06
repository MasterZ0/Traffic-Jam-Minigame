using Hasbro.TheGameOfLife.Shared;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Car
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(CarData), fileName = "New" + nameof(CarData))]
    public class CarData : ScriptableObject
    {
        [Header("Move")]
        [SerializeField] private float speedMax = 70f;
        [SerializeField] private float speedMin = -50f;
        [SerializeField] private float acceleration = 30f;
        [SerializeField] private float brakeSpeed = 100f;
        [SerializeField] private float reverseSpeed = 30f;
        [SerializeField] private float idleSlowdown = 10f;

        [Header("Turn")]
        [SerializeField] private float turnSpeedMax = 300f;
        [SerializeField] private float turnSpeedAcceleration = 300f;
        [SerializeField] private float turnIdleSlowdown = 500f;
        [SerializeField] private float minTurnAmount = 20f;

        public float SpeedMax => speedMax;
        public float SpeedMin => speedMin;
        public float Acceleration => acceleration;
        public float BrakeSpeed => brakeSpeed;
        public float ReverseSpeed => reverseSpeed;
        public float IdleSlowdown => idleSlowdown;
        public float TurnSpeedMax => turnSpeedMax;
        public float TurnSpeedAcceleration => turnSpeedAcceleration;
        public float TurnIdleSlowdown => turnIdleSlowdown;
        public float MinTurnAmount => minTurnAmount;
    }
}
