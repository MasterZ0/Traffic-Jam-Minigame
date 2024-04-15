using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(CarTargetFollowerConfig), fileName = "New" + nameof(CarTargetFollowerConfig))]
    public class CarTargetFollowerConfig : ScriptableObject
    {
        [SerializeField] private float reachedTargetDistance = 15f;
        [SerializeField] private float reverseDistance = 25f;
        [SerializeField] private float detectWallRayLength = 3f;
        [Range(0f, 180f)]
        [SerializeField] private float maxAngleForGradualTurn = 30f;

        [SerializeField] private float stoppingSpeed = 4f;
        [SerializeField] private float brakingThresholdTime = 2f;

        public float ReachedTargetDistance => reachedTargetDistance;
        public float ReverseDistance => reverseDistance;
        public float DetectWallRayLength => detectWallRayLength;
        public float MaxAngleForGradualTurn => maxAngleForGradualTurn;

        public float BrakingThresholdTime => brakingThresholdTime;
        public float StoppingSpeed => stoppingSpeed;
    }
}