using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    public abstract class CarController : MonoBehaviour, ICarController
    {
        [Header("Car Controller")]
        [SerializeField] protected CarPawn carPawn;

        public virtual float Movement { get; protected set; }
        public virtual float Direction { get; protected set; }
        public virtual bool Brake { get; protected set; }
        public bool Active { get; protected set; } = true;
        public CarPawn Pawn => carPawn;

        public virtual void SetControllerActive(bool active)
        {
            Active = active;
            carPawn.ChangeControllerState(active);
        }

        protected virtual void Awake()
        {
            if (!carPawn)
                return;

            carPawn.Possess(this);
        }

        public virtual void SetPawn(CarPawn carPawn)
        {
            transform.SetParent(carPawn.transform);
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;

            this.carPawn = carPawn;
            carPawn.Possess(this);
        }
    }

    public abstract class CarController<TCarPawn> : CarController where TCarPawn : CarPawn
    {
        public new TCarPawn Pawn => (TCarPawn)carPawn;
    }
}
