using Marmalade.TheGameOfLife.Shared;
using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class BlackCar : MonoBehaviour
    {
        [Inject]
        private TrafficJamConfig config;

        private void Awake() => ServiceLocator.GetService<TrafficJamConfig>();

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ICashHandler player))
            {
                player.RemoveCash(config.LossCashByCollision);
            }
        }
    }
}
