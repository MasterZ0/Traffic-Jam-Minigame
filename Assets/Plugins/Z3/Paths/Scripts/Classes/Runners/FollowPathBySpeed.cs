using System;
using UnityEngine;

namespace Z3.Paths
{
    public class FollowPathBySpeed : IPathRunner, IDisposable
    {
        [Header("Settings")]
        public PathPack PathPack { get; set; }
        public Transform Transfom { get; set; }
        public Func<Vector3, Vector3> GetPoint { get; set; }

        [Header("Config")]
        public float Speed { get; set; }
        public int PointsIndex { get; set; }
        public bool Reverse { get; set; } = false;
        public bool Loop { get; set; } = false;
        public bool StartInPath { get; set; } = true;


        private Vector3 targetPosition;

        public event Action OnFinish;

        public FollowPathBySpeed() { }

        public FollowPathBySpeed(Transform fromReference)
        {
            SetReference(fromReference);
        }

        public FollowPathBySpeed(Vector3 fromReference, Quaternion rotation)
        {
            SetReference(fromReference, rotation);
        }

        public void SetReference(Transform fromReference)
        {
            GetPoint = (point) => fromReference.TransformPoint(point);
        }

        public void SetReference(Vector3 positionReference, Quaternion rotationReference)
        {
            GetPoint = (point) => TransformPoint(point, positionReference, rotationReference);
        }

        public static Vector3 TransformPoint(Vector3 point, Vector3 positionReference, Quaternion rotationReference) // TODO: Move to utils
        {
            Vector3 rotatedPoint = rotationReference * point;
            return rotatedPoint + positionReference;
        }

        public void Dispose()
        {
            OnFinish = null;
            GetPoint = null;
        }

        public void Reset() => PointsIndex = 0;

        public void Start()
        {
            if (StartInPath)
            {
                Transfom.position = GetPoint(PathPack.pathPoints[PointsIndex]);
                UpdateTargetPosition();
            }
            else
            {
                targetPosition = GetPoint(PathPack.pathPoints[PointsIndex]);
            }
        }

        public void Update()
        {
            float deltaSpeed = Speed * Time.fixedDeltaTime;
            Vector3 currentPosition = Transfom.position;

            //If distance to move per update is greater than minimal distance, go to the next point
            if (deltaSpeed > Vector3.Distance(currentPosition, targetPosition))
            {
                do
                {
                    currentPosition = targetPosition;
                    UpdateTargetPosition();
                    deltaSpeed -= PathPack.spaceBetweenPoints * Time.fixedDeltaTime;

                } while (deltaSpeed > PathPack.spaceBetweenPoints);
            }

            Transfom.position = Vector3.MoveTowards(currentPosition, targetPosition, deltaSpeed);
        }

        private void UpdateTargetPosition()
        {
            if (Reverse)
            {
                PointsIndex--;
                if (PointsIndex < 0)
                {
                    PointsIndex = PathPack.pathPoints.Count - 1;
                    if (!Loop)
                    {
                        Finish();
                        return;
                    }
                }
            }
            else
            {
                PointsIndex++;
                if (PointsIndex > PathPack.pathPoints.Count - 1)
                {
                    PointsIndex = 0;
                    if (!Loop)
                    {
                        Finish();
                        return;
                    }
                }
            }

            targetPosition = GetPoint(PathPack.pathPoints[PointsIndex]);
        }

        private void Finish()
        {
            Transfom.position = targetPosition;
            OnFinish?.Invoke();
        }
    }
}