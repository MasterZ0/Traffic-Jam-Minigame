using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    public class TestCarTargetFollower : CarTargetFollower<CarPawn>
    {
        [Header("Test")]
        [SerializeField] private Transform targetPositionTranform;

        protected override Vector3 GetTargetPosition() => targetPositionTranform.position;
    }
}