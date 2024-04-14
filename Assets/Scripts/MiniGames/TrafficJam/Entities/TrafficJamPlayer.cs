using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Gameplay;
using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class TrafficJamPlayer
    {
        public Player Player { get; }
        public int Cash { get; private set; }
        public Transform Transform => CarController.Pawn.transform;

        public event Action OnUpdateCash;

        private CarController<TrafficJamCarPawn> CarController { get; }

        internal TrafficJamPlayer(Player player, CarController<TrafficJamCarPawn> carController, TrafficJamCarPawn carPawn)
        {
            Player = player;
            CarController = carController;

            carController.SetControllerActive(false);
            carController.SetPawn(carPawn);
            carPawn.SetPlayer(this);
        }

        internal void StartGame()
        {
            CarController.SetControllerActive(true);
        }

        internal void FinishGame()
        {
            CarController.SetControllerActive(false);
            CarController.Pawn.StopCompletely();
        }

        internal void AddCash(int amount)
        {
            Cash += amount;
            OnUpdateCash?.Invoke();
        }

        internal int RemoveCash(int amount)
        {
            if (Cash <= 0)
                return 0;

            int amountToRemove = Cash < amount ? Cash : amount;
            Cash -= amountToRemove;

            OnUpdateCash?.Invoke();

            return amountToRemove;
        }

        internal void Dispose()
        {
            OnUpdateCash = null;
            Object.Destroy(CarController);
        }
    }
}
