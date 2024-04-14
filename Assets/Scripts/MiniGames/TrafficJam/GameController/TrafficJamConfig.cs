using Marmalade.TheGameOfLife.Shared;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(TrafficJamConfig), fileName = "New" + nameof(TrafficJamConfig))]
    public class TrafficJamConfig : ScriptableObject
    {
        [Header("Arena")]
        [SerializeField] private int secondsToStart = 3;
        [SerializeField] private float gameDuration = 30f;

        [Header("Cash")]
        [SerializeField] private float spawRadius = 8f;
        [SerializeField] private float areaToCheckPlayer = 1f;
        [SerializeField] private float cashSpawFrequency = 3f;
        [SerializeField] private int maxSpawnedCash = 6;
        [Space]
        [Range(0f, 100f)]
        [SerializeField] private float oneThousendChangeWeight = 70f;
        [Range(0f, 100f)]
        [SerializeField] private float fiveThousandChangeWeight = 60f;
        [Range(0f, 100f)]
        [SerializeField] private float tenThousandChangeWeight = 30f;
        [Range(0f, 100f)]
        [SerializeField] private float fiftyThousandChangeWeight = 20f;

        [Header("Black Car")]
        [MinMaxSlider(1f, 20f, true)]
        [SerializeField] private Vector2 timeToSpawnBlackCar = new Vector2(2f, 6f);
        [SerializeField] private float blackCarSpawnDelay = 2f;
        [SerializeField] private int lossCashByCollision = 10000;

        [Header("Score Animations")]
        [Range(0f, 100f), SuffixLabel("%")]
        [SerializeField] private float scoreAmountPerTick = 10f;
        [Range(0.02f, 1f)]
        [SerializeField] private float scoreDelayPerTick = 0.04f;

        [Header("Score Result")]
        [SerializeField] private float scoreInitialDelay = 3f;
        [SerializeField] private float scoreAnimationDuration = 5f;
        [SerializeField] private float delayToChangeScene = 5f;

        // Arena
        public float GameDuration => gameDuration;
        public int SecondsToStart => secondsToStart;

        // Cash
        public float SpawRadius => spawRadius;
        public float CashSpawFrequency => cashSpawFrequency;
        public int MaxSpawnedCash => maxSpawnedCash;
        public float AreaToCheckPlayer => areaToCheckPlayer;

        public float OneThousendChangeWeight => oneThousendChangeWeight;
        public float FiveThousandChangeWeight => fiveThousandChangeWeight;
        public float TenThousandChangeWeight => tenThousandChangeWeight;
        public float FiftyThousandChangeWeight => fiftyThousandChangeWeight;

        // Black Car
        public Vector2 TimeToSpawnBlackCar => timeToSpawnBlackCar;
        public float BlackCarSpawnDelay => blackCarSpawnDelay;
        public int LossCashByCollision => lossCashByCollision;

        // Score Animations
        public float ScoreAmountPerTick => scoreAmountPerTick;
        public float ScoreDelayPerTick => scoreDelayPerTick;

        // Score Result
        public float ScoreInitialDelay => scoreInitialDelay;
        public float ScoreAnimationDuration => scoreAnimationDuration;
        public float DelayToChangeScene => delayToChangeScene;
    }
}
