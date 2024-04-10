using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(TrafficJamConfig), fileName = "New" + nameof(TrafficJamConfig))]
    public class TrafficJamConfig : ScriptableObject
    {
        [Header("Arena")]
        [SerializeField] private float spawRadius = 82f;
        [SerializeField] private float cashSpawFrequency = 2f;
        [SerializeField] private int maxSpawnedCash = 6;
        [SerializeField] private float gameDuration = 30f;
        [SerializeField] private int secondsToStart = 3;
        [SerializeField] private float areaToCheckPlayer = 10f;

        [Header("Black Car")]
        [SerializeField] private Vector2 timeToSpawnBlackCar = new Vector2(2f, 6f);
        [SerializeField] private float blackCarSpawnDelay;

        [Header("Score Animations")]
        [Range(0f, 100f)]
        [SerializeField] private float scoreAmountPerTick = 10f;
        [Range(0.02f, 1f)]
        [SerializeField] private float scoreDelayPerTick = 0.04f;

        [Header("Cash Probabilities")]
        [Range(0f, 100f)]
        [SerializeField] private float oneThousendChangeWeight = 70f;
        [Range(0f, 100f)]
        [SerializeField] private float fiveThousandChangeWeight = 60f;
        [Range(0f, 100f)]
        [SerializeField] private float tenThousandChangeWeight = 30f;
        [Range(0f, 100f)]
        [SerializeField] private float fiftyThousandChangeWeight = 20f;
        [Space]
        [SerializeField] private int lossCashByCollision = 10000;

        public float SpawRadius => spawRadius;
        public float GameDuration => gameDuration;
        public int SecondsToStart => secondsToStart;

        public float OneThousendChangeWeight => oneThousendChangeWeight;
        public float FiveThousandChangeWeight => fiveThousandChangeWeight;
        public float TenThousandChangeWeight => tenThousandChangeWeight;
        public float FiftyThousandChangeWeight => fiftyThousandChangeWeight;
        public int LossCashByCollision => lossCashByCollision;

        public float CashSpawFrequency => cashSpawFrequency;
        public int MaxSpawnedCash => maxSpawnedCash;
        public float AreaToCheckPlayer => areaToCheckPlayer;

        public Vector2 TimeToSpawnBlackCar => timeToSpawnBlackCar;
        public float BlackCarSpawnDelay => blackCarSpawnDelay;

        public float ScoreAmountPerTick => scoreAmountPerTick;
        public float ScoreDelayPerTick => scoreDelayPerTick;
    }
}
