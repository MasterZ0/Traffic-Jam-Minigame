using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(CarTargetFollowerConfig), fileName = "New" + nameof(CarTargetFollowerConfig))]
    public class CarTargetFollowerConfig : ScriptableObject, IService
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