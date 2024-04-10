using UnityEngine;

namespace Z3.Paths
{
    /// <summary>
    /// Note to developers: Please describe what this MonoBehaviour does.
    /// </summary>
    public class BezierReference : MonoBehaviour 
    {
        [Header("Bezier Reference")]
        [SerializeField] private BezierCurve bezierCurve = new BezierCurve(Vector2.zero);

        public BezierCurve BezierCurve { get => bezierCurve; set => bezierCurve = value; }
    }
}