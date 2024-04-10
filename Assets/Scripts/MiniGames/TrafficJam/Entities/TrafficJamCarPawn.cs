using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Gameplay;
using Marmalade.TheGameOfLife.Shared;
using System;
using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public interface ICashHandler
    {
        event Action OnUpdateCash;

        int Cash { get; }

        void AddCash(int amount);
        void RemoveCash(int amount);
        Color GetPlayerColor();
    }

    public class TrafficJamCarPawn : CarPawn, ICashHandler, IEntity
    {
        [Header("Traffic Jam Car")]
        [SerializeField] private CharacterColor carColor;
        [SerializeField] private Transform center;

        public event Action OnUpdateCash;

        public CharacterColor CarColor => carColor;
        public int Cash {  get; private set; }

        private Player Player { get; set; }

        public Rigidbody AttachedRigidbody => carRigidbody;
        public Transform Center => center;

        internal void SetPlayer(CarController<TrafficJamCarPawn> carController, Player player)
        {
            Cash = 0;
            Player = player;
            carController.SetPawn(this);
        }

        public Color GetPlayerColor() => Player.CharacterColor.GetColor();

        public void AddCash(int amount)
        {
            Cash += amount;
            OnUpdateCash?.Invoke();
        }

        public void RemoveCash(int amount)
        {
            if (Cash == 0)
                return;

            Cash -= amount;
            if (Cash < 0)
            {
                Cash = 0;
            }

            OnUpdateCash?.Invoke();
        }

        private void OnDestroy()
        {
            OnUpdateCash = null;
        }
    }
}
