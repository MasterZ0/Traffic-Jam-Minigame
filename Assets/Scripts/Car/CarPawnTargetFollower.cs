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

        public override void ChangeControllerState(bool active)
        {
            base.ChangeControllerState(active);

            if (active)
            {
                targetAnimator.transform.SetParent(null);
            }
            else
            {
                targetAnimator.transform.SetParent(transform);
            }
        }
    }
}