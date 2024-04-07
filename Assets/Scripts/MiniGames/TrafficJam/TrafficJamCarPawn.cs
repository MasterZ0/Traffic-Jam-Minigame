using Hasbro.TheGameOfLife.Car;
using Hasbro.TheGameOfLife.Gameplay;
using Hasbro.TheGameOfLife.Shared;
using System;
using UnityEngine;

namespace Hasbro.TheGameOfLife.TrafficJam
{
    public interface ICashHandler
    {
        event Action OnUpdateCash;

        int Cash { get; }

        void AddCash(int amount);
        void RemoveCash(int amount);
        Color GetPlayerColor();
    }

    public class TrafficJamCarPawn : CarPawn, ICashHandler
    {
        [SerializeField] private CharacterColor carColor;

        public event Action OnUpdateCash;

        public CharacterColor CarColor => carColor;
        public int Cash {  get; private set; }

        private Player Player { get; set; }

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
