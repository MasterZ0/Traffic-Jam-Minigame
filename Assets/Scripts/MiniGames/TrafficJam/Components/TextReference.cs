using TMPro;
using UnityEngine;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class TextReference : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;

        private Transform target;

        public void Init(Transform target, string value)
        {
            this.target = target;
            text.text = value;
        }

        private void Update()
        {
            transform.position = target.position;
        }
    }
}
