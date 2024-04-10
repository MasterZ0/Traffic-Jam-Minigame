using System.Collections.Generic;
using UnityEngine;

namespace Z3.Paths
{
    [CreateAssetMenu(menuName = "Z3/Paths/Path Pack", fileName = "New" + nameof(PathPack))]
    public class PathPack : ScriptableObject
    {
        public float spaceBetweenPoints = 0.2f;
        public bool loop;
        public List<BezierCurve> curves = new List<BezierCurve>();

        public List<Vector3> pathPoints = new List<Vector3>();
        public float pathLength;
    }
}