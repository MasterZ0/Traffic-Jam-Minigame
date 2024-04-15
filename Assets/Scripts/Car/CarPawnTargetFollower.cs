using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    public class CarPawnTargetFollower : CarPawn
    {
        [Header("Target Follower")]
        [SerializeField] private Transform leftFrontDetector;
        [SerializeField] private Transform rightFrontDetector;
        [SerializeField] private Animator targetAnimator;

        [Header("Gameplay")]
        [SerializeField] private LayerMask scenaryLayer;

        [Header("Animator States")]
        [SerializeField] private string activeParameter = "Active";

        public Transform LeftFrontDetector => leftFrontDetector;
        public Transform RightFrontDetector => rightFrontDetector;
        public LayerMask ScenaryLayer => scenaryLayer;

        public void ActiveTargetAnimator(bool active)
        {
            targetAnimator.SetBool(activeParameter, active);
        }

        public void SetTargetPosition(Vector3 targetPosition)
        {
            targetAnimator.transform.position = targetPosition;
        }

        public override void Possess(ICarController controller)
        {
            base.Possess(controller);

            targetAnimator.transform.SetParent(null);
        }

        public override void RemoveController()
        {
            base.RemoveController();

            targetAnimator.transform.SetParent(transform);
        }
    }
}