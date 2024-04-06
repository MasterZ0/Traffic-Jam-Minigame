using Hasbro.TheGameOfLife.Shared;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Car
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(CarTargetFollowerData), fileName = "New" + nameof(CarTargetFollowerData))]
    public class CarTargetFollowerData : ScriptableObject
    {
        [SerializeField] private float stoppingDistance = 30f;
        [SerializeField] private float stoppingSpeed = 40f;
        [SerializeField] private float reachedTargetDistance = 15f;
        [SerializeField] private float reverseDistance = 25f;

        public float StoppingDistance => stoppingDistance;
        public float StoppingSpeed => stoppingSpeed;
        public float ReachedTargetDistance => reachedTargetDistance;
        public float ReverseDistance => reverseDistance;
    }
}