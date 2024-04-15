using Marmalade.TheGameOfLife.Car;
using Marmalade.TheGameOfLife.Shared;
using Z3.Utils.ExtensionMethods;
using UnityEngine;
using Z3.ObjectPooling;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class TrafficJamCarPawn : CarPawnTargetFollower, ICashHandler, IEntity
    {
        [Header("Traffic Jam Car")]
        [SerializeField] private CharacterColor carColor;
        [SerializeField] private Transform center;
        [SerializeField] private Transform recoveryFX;

        public CharacterColor CarColor => carColor;
        public Rigidbody AttachedRigidbody => carRigidbody;
        public Transform Center => center;

        private TrafficJamPlayer player;
        private Vector3 startPosition;
        private Quaternion startRotation;

        private float invalidTime;

        [Inject]
        private TrafficJamConfig Config { get; set; }

        internal void SetPlayer(TrafficJamPlayer player)
        {
            startPosition = transform.position;
            startRotation = transform.rotation;
            this.player = player;

            ActiveCar();
        }

        public void AddCash(int amount) => player.AddCash(amount);

        public int RemoveCash(int amount) => player.RemoveCash(amount);

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            float xRotation = transform.eulerAngles.x.NormalizeAngle();
            float zRotation = transform.eulerAngles.z.NormalizeAngle();

            bool overturn = Mathf.Abs(xRotation) > Config.MaxCarRotation || Mathf.Abs(zRotation) > Config.MaxCarRotation;

            float distanceFromCenter = Vector3.Distance(transform.position, Vector3.zero);
            bool outsideTheArena = distanceFromCenter > Config.ArenaRadius;

            if (!outsideTheArena && !overturn)
            {
                invalidTime = 0;
                return;
            }

            invalidTime += Time.fixedDeltaTime;

            if (invalidTime > Config.MaxInvalidCarTime)
            {
                transform.position = startPosition;
                transform.rotation = startRotation;

                recoveryFX.SpawnPooledObject(startPosition, startRotation);
            }
        }
    }
}
