using System;
using UnityEngine;

namespace Z3.Paths
{
    public class BezierMovement : IPathRunner
    {
        public Transform Target { get; set; }
        public BezierReference BezierReference { get; set; }
        public float Speed { get; set; }

        private BezierCurve bezierCurve;
        private Vector2 referencePosition;
        private float transition;
        private float curveLength;

        public event Action OnFinish;

        public BezierMovement(Transform target, BezierReference bezierReference, float speed)
        {
            Target = target;
            BezierReference = bezierReference;
            Speed = speed;
        }

        public void Start()
        {
            referencePosition = BezierReference.transform.position;
            bezierCurve = BezierReference.BezierCurve;

            transition = 0;
            curveLength = bezierCurve.CalculateCurveLength();
        }


        public void Update()
        {
            transition += Time.fixedDeltaTime * Speed / curveLength;
            Vector2 nextPosition = bezierCurve.GetTransitionPoint(transition);
            Target.position = referencePosition + nextPosition;

            if (transition >= 1)
            {
                OnFinish?.Invoke();
            }
        }

        public void Dispose()
        {
            OnFinish = null;
        }
    }
}