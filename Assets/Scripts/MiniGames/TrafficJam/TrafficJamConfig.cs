using Hasbro.TheGameOfLife.Shared;
using UnityEngine;

namespace Hasbro.TheGameOfLife.TrafficJam
{
    [CreateAssetMenu(menuName = ProjectPath.ScriptableObjects + nameof(TrafficJamConfig), fileName = "New" + nameof(TrafficJamConfig))]
    public class TrafficJamConfig : ScriptableObject, IService
    {
        [Header("Arena")]
        [SerializeField] private float spawRadius = 82f;
        [SerializeField] private float cashSpawFrequency = 2f;
        [SerializeField] private int maxSpawnedCash = 6;
        [SerializeField] private float gameDuration = 30f;
        [SerializeField] private int lossCashByCollision = 10000;

        [Header("Cash Probabilities")]
        [Range(0f, 100f)]
        [SerializeField] private float oneThousendChangeWeight = 70f;
        [Range(0f, 100f)]
        [SerializeField] private float fiveThousandChangeWeight = 60f;
        [Range(0f, 100f)]
        [SerializeField] private float tenThousandChangeWeight = 30f;
        [Range(0f, 100f)]
        [SerializeField] private float fiftyThousandChangeWeight = 20f;

        public float SpawRadius => spawRadius;
        public float GameDuration => gameDuration;

        public float OneThousendChangeWeight => oneThousendChangeWeight;
        public float FiveThousandChangeWeight => fiveThousandChangeWeight;
        public float TenThousandChangeWeight => tenThousandChangeWeight;
        public float FiftyThousandChangeWeight => fiftyThousandChangeWeight;

        public float CashSpawFrequency => cashSpawFrequency;
        public int MaxSpawnedCash => maxSpawnedCash;
        public int LossCashByCollision => lossCashByCollision;
    }
}
