using Marmalade.TheGameOfLife.Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    /// <summary>
    /// Abstract class that controls a <see cref="CarPawn"/> to follow a target position by <see cref="Vector3"/>.
    /// </summary>
    public abstract class CarControllerTargetFollower : CarController<CarPawnTargetFollower>
    {
        [ShowInInspector, ReadOnly]
        protected bool ReachedTarget { get; private set; }

        [Inject] private CarTargetFollowerConfig config;

        private Transform LeftFrontDetector => Pawn.LeftFrontDetector;
        private Transform RightFrontDetector => Pawn.RightFrontDetector;
        private LayerMask ScenaryLayer => Pawn.ScenaryLayer;

        private float RayLength => config.DetectWallRayLength;

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
            float forwardAmount = 0f;
            float turnAmount = 0f;
            Vector3 targetPosition = GetTargetPosition();

            float distanceToTarget = Vector3.Distance(transform.position, targetPosition);
            ReachedTarget = distanceToTarget <= config.ReachedTargetDistance;

            if (!ReachedTarget)
            {
                Vector3 targetDirection = (targetPosition - transform.position).normalized;

                float dot = Vector3.Dot(transform.forward, targetDirection);
                bool targetForward = dot >= 0f;

                if (targetForward)
                {
                    float remainingTime = distanceToTarget / carPawn.Speed;

                    if (remainingTime <= config.BrakingThresholdTime)
                    {
                        // Slow down
                        float speedRatio = carPawn.Speed / config.StoppingSpeed;

                        float amount = Mathf.Lerp(1.0f, -1.0f, speedRatio);
                        forwardAmount = Mathf.Clamp(amount, -1f, 1f);
                    }
                    else
                    {
                        // Continue moving at full speed
                        forwardAmount = 1f;
                    }
                }
                else
                {
                    // Check for wall
                    bool hitLeft = Physics.Raycast(LeftFrontDetector.position, LeftFrontDetector.forward, RayLength, ScenaryLayer);
                    bool hitRight = Physics.Raycast(RightFrontDetector.position, RightFrontDetector.forward, RayLength, ScenaryLayer);

                    if (hitLeft || hitRight)
                    {
                        forwardAmount = -1;
                        targetDirection *= -1;
                    }
                    else
                    {
                        forwardAmount = distanceToTarget > config.ReverseDistance ? 1f : -1f;
                    }
                }

                float angleToDirection = Vector3.SignedAngle(transform.forward, targetDirection, Vector3.up);
                turnAmount = Mathf.Clamp(angleToDirection / config.MaxAngleForGradualTurn, -1f, 1f);
            }

            Direction = turnAmount;
            Movement = forwardAmount;
            Brake = ReachedTarget;
        }

        private void OnDrawGizmosSelected()
        {
            if (Pawn == null)
                return;

            if (config == null)
            {
                this.InjectServices();
                return;
            }

            bool isHitLeft = Physics.Raycast(LeftFrontDetector.position, LeftFrontDetector.forward, out RaycastHit leftRay, RayLength, ScenaryLayer);
            bool isHitRight = Physics.Raycast(RightFrontDetector.position, RightFrontDetector.forward, out RaycastHit rightRay, RayLength, ScenaryLayer);

            Vector3 leftTarget = isHitLeft ? leftRay.point : LeftFrontDetector.position + LeftFrontDetector.forward * RayLength;
            Gizmos.color = isHitLeft ? Color.red : Color.green;
            Gizmos.DrawLine(LeftFrontDetector.position, leftTarget);

            Vector3 rightTarget = isHitRight ? rightRay.point : RightFrontDetector.position + RightFrontDetector.forward * RayLength;
            Gizmos.color = isHitRight ? Color.red : Color.green;
            Gizmos.DrawLine(RightFrontDetector.position, rightTarget);

            if (!Application.isPlaying)
                return;

            Vector3 target = GetTargetPosition();

            Gizmos.color = ReachedTarget ? Color.blue : Color.red;
            Gizmos.DrawLine(transform.position, target);
            Gizmos.DrawWireSphere(target, config.ReachedTargetDistance);
        }
    }
    /*
    public class DrivingState
    {
        bool EvadingBlocked;

        public override void UpdateState()
        {
            if (IsFrontHittingWall() && !EvadingBlocked)
            {
                SwitchState<EvadingState>();
                return;
            }

            float forwardAmount = Vector3.Distance(CarTransform.position, Target) > Data.ReachedTargetDistance ? 1f : 0f;

            if (forwardAmount != 0)
            {
                Vector3 dirToTarget = (Target - CarTransform.position).normalized;
                float angleToDir = Vector3.SignedAngle(CarTransform.forward, dirToTarget, Vector3.up);
                float turnDirection = angleToDir > 0 ? 1f : -1f;

                bool leftObstacle = CastRayForObstacle(CarPawn.LeftHeadeLight.position);
                bool rightObstacle = CastRayForObstacle(CarPawn.LeftHeadeLight.position);
                if (leftObstacle || rightObstacle)
                {
                    turnDirection = leftObstacle ? 1f : -1f;
                }

                carController.SetDirection(turnDirection);
            }

            carController.SetMovement(forwardAmount);
        }



        private bool CastRayForObstacle(Vector3 origin)
        {
            return Physics.Raycast(origin, CarTransform.forward, Data.ObstacleDetectionDistance, Data.ObstacleLayer);
        }
    }

    public class EvadingState : CarState
    {
        private CancellationTokenSource cancellationToken = new CancellationTokenSource();

        private Quaternion startRotation;
        private float evasionAngleSum;

        public override void EnterState()
        {
            StateMachine.EvadingBlocked = true;

            carController.SetMovement(-1f);
            startRotation = CarTransform.rotation;
            evasionAngleSum = 0f;
        }

        public override void UpdateState()
        {
            if (evasionAngleSum >= Data.RequiredEvasionAngle)
            {
                SwitchState<DrivingState>();
            }
            else
            {
                carController.SetMovement(-1f);

                Vector3 toTarget = (Target - CarTransform.position).normalized;
                float angleToTarget = Vector3.SignedAngle(CarTransform.forward, toTarget, Vector3.up);
                carController.SetDirection(angleToTarget > 0 ? -1f : 1f);

                float currentAngle = Quaternion.Angle(startRotation, CarTransform.rotation);
                evasionAngleSum += currentAngle;
                startRotation = CarTransform.rotation;
            }
        }

        public override void ExitState()
        {
            cancellationToken?.Cancel();
            cancellationToken = new CancellationTokenSource();
            UniTask.Create(() => EndEvading(cancellationToken.Token));
        }

        private async UniTask EndEvading(CancellationToken token)
        {
            await UniTask.WaitForSeconds(Data.TimeEntryEvadingAgain);
            token.ThrowIfCancellationRequested();
            StateMachine.EvadingBlocked = false;
        }
    }*/
}