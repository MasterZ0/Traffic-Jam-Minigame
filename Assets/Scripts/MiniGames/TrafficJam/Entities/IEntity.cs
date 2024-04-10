using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public interface IEntity
    {
        Rigidbody AttachedRigidbody { get; }
        Transform Center { get; }
    }
}
