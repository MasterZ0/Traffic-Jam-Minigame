using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    /// <summary>
    /// Abstract class that controls a <see cref="CarPawn"/> to follow a target position by <see cref="Vector3"/>.
    /// </summary>
    public abstract class CarTargetFollower<TCarPawn> : CarController<TCarPawn> where TCarPawn : CarPawn
    {
        [Inject] private CarTargetFollowerConfig data;

        protected abstract Vector3 GetTargetPosition();

        protected virtual void OnEnable()
        {
            this.InjectServices();
        }

        protected virtual void FixedUpdate()
        {
            UpdateController();
        }

        private void UpdateController()
        {
            float forwardAmount;
            float turnAmount;
            Vector3 targetPosition = GetTargetPosition();

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            if (distanceToTarget > data.ReachedTargetDistance)
            {
                // Still too far, keep going
                Vector3 dirToMovePosition = (targetPosition - transform.position).normalized;
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

        private void OnDrawGizmosSelected()
        {
            if (!Application.isPlaying)
                return;

            Vector3 target = GetTargetPosition();

            Gizmos.color = Color.red;
            Gizmos.DrawLine(target, transform.position);
            Gizmos.DrawSphere(target, .5f);
        }
    }
}