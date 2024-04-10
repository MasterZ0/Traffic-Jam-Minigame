using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class UpdateForwardRotation : MonoBehaviour
    {
        private Vector3 lastPosition;

        void OnEnable()
        {
            lastPosition = transform.position;
        }

        void Update()
        {
            if (transform.position == lastPosition)
                return;
            
            Vector3 moveDirection = transform.position - lastPosition;

            if (moveDirection != Vector3.zero)
            {
                transform.forward = moveDirection.normalized;
            }

            lastPosition = transform.position;
        }
    }
}
