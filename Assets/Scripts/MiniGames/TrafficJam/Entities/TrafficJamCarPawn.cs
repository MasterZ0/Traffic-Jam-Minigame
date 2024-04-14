using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class TrafficJamCarPawn : CarPawnTargetFollower, ICashHandler, IEntity
    {
        [Header("Traffic Jam Car")]
        [SerializeField] private CharacterColor carColor;
        [SerializeField] private Transform center;

        public CharacterColor CarColor => carColor;
        public Rigidbody AttachedRigidbody => carRigidbody;
        public Transform Center => center;

        private TrafficJamPlayer Player { get; set; }

        internal void SetPlayer(TrafficJamPlayer player)
        {
            Player = player;
            ActiveCar();
        }

        public void AddCash(int amount) => Player.AddCash(amount);

        public int RemoveCash(int amount) => Player.RemoveCash(amount);
    }
}
