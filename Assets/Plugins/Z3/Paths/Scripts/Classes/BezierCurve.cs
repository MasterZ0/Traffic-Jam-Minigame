using UnityEngine;

namespace Z3.Paths
{
    [System.Serializable]
    public class BezierCurve
    {
        public Vector3 startPosition;
        public Vector3 startTangent;

        public Vector3 endTangent;
        public Vector3 endPosition;

        private const int Resolution = 20;

        public BezierCurve() { }

        public BezierCurve(Vector3 reference)
        {
            startPosition = reference;

            startTangent = reference;
            startTangent.x += .5f;

            endTangent = reference;
            endTangent.x += 1.5f;

            endPosition = reference;
            endPosition.x += 2f;
        }

        public Vector3 GetTransitionPoint(float transition)
        {
            return CalculateCubicBezierPoint(transition, startPosition, startTangent, endTangent, endPosition);
        }

        /// <summary>
        /// Return the total size of the bezier curve.
        /// </summary>
        /// <remarks>The resolution defines how many points there will be. The bigger the resolution,
        /// the closer it gets from the real length.</remarks>
        /// <param name="resolution">The amount of points in the length calculation.</param>
        public float CalculateCurveLength(int resolution = Resolution)
        {
            float transitionSize = 1f / resolution;
            float length = 0;
            Vector3 previousPosition = startPosition;

            for (int i = 1; i <= resolution; i++)
            {
                float transition = i * transitionSize;
                Vector3 newPosition = CalculateCubicBezierPoint(transition, startPosition, startTangent, endTangent, endPosition);
                length += Vector3.Distance(previousPosition, newPosition);
                previousPosition = newPosition;
            }

            return length;
        }

        /// <summary>
        /// Calculate = u³ * P0 + 3 * u² * t * P1 + 3 * u * t² * P2 + t³ * P3
        /// </summary>
        /// <remarks>
        /// u = (1 - t)
        /// </remarks>
        public static Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
        {
            float u = 1 - t;
            float uu = u * u;
            float uuu = uu * u;

            float tt = t * t;
            float ttt = tt * t;

            return (uuu * p0) + (3 * uu * t * p1) + (3 * u * tt * p2) + (ttt * p3);
        }
    }
}