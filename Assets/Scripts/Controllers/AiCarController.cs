using Hasbro.TheGameOfLife.Car;
using UnityEngine;

namespace Hasbro.TheGameOfLife.Controllers
{
    public class AiCarController : CarTargetFollower
    {
        [Header("AI")]
        [SerializeField] private Transform targetPositionTranform;

        protected override Vector3 TargetPosition => targetPositionTranform.position;


        protected override void Awake()
        {
            base.Awake();

            // Get Gameplay events
        }
    }
}