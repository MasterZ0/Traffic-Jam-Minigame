using UnityEngine;

namespace Z3.Paths
{
    public static class PathUtils
    {
        /// <summary>
        /// Maybe there are more interesting ways to do this
        /// </summary>
        public static int GetClosestPathPoint(Vector3 from, PathReference pathReference)
        {
            PathPack pack = pathReference.PathPack;
            float bestDistance = float.PositiveInfinity;

            // Find the closest StartPoint
            int bestIndex = -1;
            for (int i = 0; i < pack.pathPoints.Count; i++)
            {
                float distance = Vector3.Distance(from, pathReference.transform.TransformPoint(pack.pathPoints[i]));
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestIndex = i;
                }
            }

            return bestIndex;
        }
    }
}