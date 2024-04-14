﻿using UnityEngine;

namespace Marmalade.TheGameOfLife.Car
{
    public abstract class CarController : MonoBehaviour
    {
        public virtual float Movement { get; protected set; }
        public virtual float Direction { get; protected set; }
        public bool Active { get; protected set; } = true;

        public virtual void SetControllerActive(bool active)
        {
            Active = active;
        }
    }

    public abstract class CarController<TCarPawn> : CarController where TCarPawn : CarPawn
    {
        [Header("Car Controller")]
        [SerializeField] protected TCarPawn carPawn;

        public TCarPawn Pawn => carPawn;

        protected virtual void Awake()
        {
            if (!carPawn)
                return;

            carPawn.Possess(this);
        }

        public void SetPawn(TCarPawn carPawn)
        {
            transform.SetParent(carPawn.transform);
            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = Vector3.zero;

            this.carPawn = carPawn;
            carPawn.Possess(this);
        }
    }
}
