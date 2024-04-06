using UnityEngine;

namespace Hasbro.TheGameOfLife.Car
{
    public abstract class CarController : MonoBehaviour
    {
        [Header("Car Controller")]
        [SerializeField] protected CarPawn carPawn;

        public virtual float Movement { get; internal set; }
        public virtual float Direction { get; internal set; }

        protected virtual void Awake()
        {
            carPawn.Possess(this);
        }
    }
}
