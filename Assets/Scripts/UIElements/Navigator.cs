using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Marmalade.UIElements
{
    public abstract class Navigator : Selectable
    {
        [Header("Navigator")]
        [SerializeField] private Button leftArrow;
        [SerializeField] private Button rightArrow;
        [SerializeField] protected TextMeshProUGUI value;

        protected abstract bool LeftEnd { get; }
        protected abstract bool RightEnd { get; }
        protected virtual bool CanLoop { get; }

        protected override void Awake()
        {
            base.Awake();

            if (!Application.isPlaying)
                return;

            leftArrow.onClick.AddListener(GoLeft);
            rightArrow.onClick.AddListener(GoRight);
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            if (!Application.isPlaying)
                return;

            UpdateView();
        }

        public override Selectable FindSelectableOnLeft()
        {
            leftArrow.OnSubmit(null);
            return null;
        }

        public override Selectable FindSelectableOnRight()
        {
            rightArrow.OnSubmit(null);
            return null;
        }

        private void GoLeft()
        {
            if (LeftEnd && !CanLoop)
                return;

            SetValue(false);
            UpdateView();
        }

        private void GoRight()
        {
            if (RightEnd && !CanLoop)
                return;

            SetValue(true);
            UpdateView();
        }

        protected void UpdateView()
        {
            if (value)
            {
                value.text = GetText();
            }

            if (CanLoop)
                return;

            leftArrow.interactable = !LeftEnd;
            rightArrow.interactable = !RightEnd;
        }

        protected abstract string GetText();
        protected abstract void SetValue(bool right);
    }
}