using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using Marmalade.TheGameOfLife.Shared;
using Z3.UnityUI;

namespace Marmalade.TheGameOfLife.MainMenu
{
    public class PlayerSelector : MonoBehaviour
    {
        [Header("Player Number")]
        [SerializeField] private int playerNumber;
        [Space]
        [SerializeField] private PlayerType playerType;
        [SerializeField] private CharacterColor characterColor;

        [Header("Player Number")]
        [SerializeField] private GameObject joinContainer;
        [SerializeField] private GameObject characterSelectionContainer;
        [SerializeField] private TMP_Text playerNameText;
        [SerializeField] private TMP_Text playerNumberText;
        [SerializeField] private NavigatorSelection colorSelection;

        [SerializeField] private Image pawn;
        [SerializeField] private Image joinBackground;

        public string PlayerName => $"{playerType} {playerNumber}";
        public PlayerType PlayerType => playerType;
        public CharacterColor CharacterColor => characterColor;

        private GeneralConfig appConfig;

        internal void Init(GeneralConfig appConfig)
        {
            this.appConfig = appConfig;

            playerNumberText.text = $"Player {playerNumber}";

            int maxColors = Enum.GetValues(typeof(CharacterColor)).Length;
            colorSelection.Init((int)characterColor, maxColors);

            UpdateView();
        }

        public void OnRemovePlayer()
        {
            playerType = PlayerType.None;
            UpdateView();
        }

        public void OnSetColor(int color)
        {
            characterColor = (CharacterColor)color;
            UpdateView();
        }

        public void OnJoinAsComputer()
        {
            playerType = PlayerType.Computer;
            UpdateView();
        }

        private void UpdateView()
        {
            playerNameText.text = PlayerName;

            Color color = appConfig.GetColor(characterColor);
            pawn.color = color;
            joinBackground.color = color;

            characterSelectionContainer.SetActive(playerType != PlayerType.None);
            joinContainer.SetActive(playerType == PlayerType.None);
        }
    }
}
