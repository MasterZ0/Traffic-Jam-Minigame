using UnityEngine;
using UnityEngine.Events;

namespace Marmalade.UIElements
{
    public class NavigatorSelection : Navigator
    {
        [Header("Selection")]
        [Tooltip("Send current index")]
        [SerializeField] public UnityEvent<int> onValueChange;

        private int maxValue;
        private int index;

        protected override bool LeftEnd => index == 0;
        protected override bool RightEnd => index == maxValue - 1;
        protected override bool CanLoop => true;

        public void Init(int currentIndex, int maxValue)
        {
            this.maxValue = maxValue;
            SetIndex(currentIndex);
        }

        public void SetIndex(int currentIndex)
        {
            if (currentIndex < 0 || currentIndex >= maxValue)
                throw new System.ArgumentOutOfRangeException($"Index out of range. Length: {maxValue}, Current Index: {currentIndex}");

            index = currentIndex;
            UpdateView();
        }

        protected override string GetText() => index.ToString();

        protected override void SetValue(bool right)
        {
            index = Navigate(index, maxValue, right);
            onValueChange.Invoke(index);
        }

        public static int Navigate(int value, int length, bool goRight)
        {
            if (goRight)
            {
                value++;
                if (value == length)
                    value = 0;
            }
            else
            {
                value--;
                if (value < 0)
                    value = length - 1;
            }
            return value;
        }
    }
}