using UnityEngine;

namespace Hasbro.TheGameOfLife.Car
{
    /// <summary>
    /// Abstract class that controls a <see cref="CarPawn"/> to follow a target position by <see cref="Vector3"/>.
    /// </summary>
    public abstract class CarTargetFollower : CarController
    {
        [Header("Target Follower")]
        [SerializeField] private CarTargetFollowerData data;

        protected abstract Vector3 TargetPosition { get; }

        protected virtual void FixedUpdate()
        {
            UpdateController();
        }

        private void UpdateController()
        {
            float forwardAmount;
            float turnAmount;

            float distanceToTarget = Vector3.Distance(transform.position, TargetPosition);
            if (distanceToTarget > data.ReachedTargetDistance)
            {
                // Still too far, keep going
                Vector3 dirToMovePosition = (TargetPosition - transform.position).normalized;
                float dot = Vector3.Dot(transform.forward, dirToMovePosition);

                if (dot > 0)
                {
                    // Target in front
                    forwardAmount = 1f;

                    if (distanceToTarget < data.StoppingDistance && carPawn.Speed > data.StoppingSpeed)
                    {
                        // Within stopping distance and moving forward too fast
                        forwardAmount = -1f;
                    }
                }
                else
                {
                    // Target behind
                    forwardAmount = distanceToTarget > data.ReverseDistance ? 1f : -1f;
                }

                float angleToDir = Vector3.SignedAngle(transform.forward, dirToMovePosition, Vector3.up);

                turnAmount = angleToDir > 0 ? 1f : -1f;
            }
            else
            {
                // Reached target
                if (carPawn.Speed > data.ReachedTargetDistance) // Review it
                {
                    forwardAmount = -1f;
                }
                else
                {
                    forwardAmount = 0f;
                }

                turnAmount = 0f;
            }

            Direction = turnAmount;
            Movement = forwardAmount;
        }
    }
}