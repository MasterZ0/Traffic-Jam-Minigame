using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Z3.Paths
{
    public class FollowPathByTime : IPathRunner
    {
        // Core
        public Transform Target { get; set; }
        public PathReference PathReference { get; set; }

        // Config
        public float Time { get; set; }
        public int PathIndex { get; set; }
        public bool Reverse { get; set; }
        public bool Loop { get; set; }

        public event Action OnFinish;

        private PathPack pathPack => PathReference.PathPack;
        private Vector3 previousPosition;
        private Vector3 nextPosition;
        private float overShootSpeed;
        private float speed;

        public void Start()
        {
            speed = pathPack.pathLength / Time;
            PathIndex = Reverse ? pathPack.pathPoints.Count - 1 : 0;
            Target.position = Reverse ? GetPoint(pathPack.pathPoints.Last()) : GetPoint(pathPack.pathPoints[0]);
            nextPosition = GetNewPosition(pathPack.pathPoints);
        }

        public void Update()
        {
            previousPosition = Target.position;
            Target.position = Vector3.MoveTowards(previousPosition, nextPosition, speed * UnityEngine.Time.fixedDeltaTime);
            overShootSpeed = speed * UnityEngine.Time.fixedDeltaTime;

            if (Vector3.Distance(previousPosition, nextPosition) <= Vector3.Distance(previousPosition, Target.position))
            {
                nextPosition = GetNewPosition(pathPack.pathPoints);
            }

            while (overShootSpeed > pathPack.spaceBetweenPoints) //If distance moved per update is greater than minimal distance, move again
            {
                previousPosition = Target.position;
                overShootSpeed -= pathPack.spaceBetweenPoints;
                nextPosition = GetNewPosition(pathPack.pathPoints);
                Target.position = Vector3.MoveTowards(previousPosition, nextPosition, overShootSpeed);
            }
        }

        private Vector3 GetNewPosition(List<Vector3> path)
        {
            PathIndex += Reverse ? -1 : +1;


            if (PathIndex > path.Count - 1)
            {
                PathIndex = 0;
                if (!Loop)
                    Finish();
            }

            if (PathIndex < 0)
            {
                PathIndex = path.Count - 1;
                if (!Loop)
                    Finish();
            }

            return GetPoint(path[PathIndex]);
        }

        private Vector3 GetPoint(Vector3 point) => PathReference.transform.TransformPoint(point);

        private void Finish() => OnFinish?.Invoke();

        public void Dispose()
        {
            OnFinish = null;
        }
    }
}