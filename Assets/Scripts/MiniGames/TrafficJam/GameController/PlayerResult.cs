using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Marmalade.TheGameOfLife.TrafficJam
{
    public class PlayerResult : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private Image sliderFill;
        [SerializeField] private Image background;
        [SerializeField] private Image icon;
        [SerializeField] private RectTransform referencePosition;

        private float animationPercentage;
        private float scorePercentage;
        private bool startAnimation;

        private TrafficJamPlayer player;
        private ScoreResult controller;

        public void Init(ScoreResult controller, TrafficJamPlayer player, float scorePercentage)
        {
            this.controller = controller;
            playerNameText.text = player.Player.DisplayName;

            Color playerColor = player.Player.Color;
            background.color = playerColor;
            sliderFill.color = playerColor;
            icon.color = playerColor;

            scoreText.text = "0";
            slider.value = 0f;

            this.player = player;

            this.scorePercentage = scorePercentage;
            UpdateCarPosition();
        }

        public void Play()
        {
            startAnimation = true;
        }

        void Update()
        {
            UpdateCarPosition();

            if (!startAnimation)
                return;

            animationPercentage += Time.deltaTime / controller.ScoreAnimationDuration;

            scoreText.text = Mathf.RoundToInt(animationPercentage * player.Cash).ToString();
            slider.value = animationPercentage * scorePercentage;

            if (animationPercentage >= 1)
            {
                scoreText.text = $"{player.Cash}";
                slider.value = scorePercentage;

                startAnimation = false;
            }
        }

        private void UpdateCarPosition()
        {
            Camera mainCamera = controller.MainCamera.Camera;

            Vector3 calculate = mainCamera.transform.TransformPoint(Vector3.forward * controller.CamOffset);  // TODO: Find a formula to calculate the offset

            Vector3 center = referencePosition.position;
            center.z = Vector3.Distance(calculate, mainCamera.transform.position);

            player.Transform.position = mainCamera.ScreenToWorldPoint(center);
            player.Transform.rotation = controller.ReferencePosition.rotation;
        }
    }
}
