using UnityEngine;
using TMPro;
using Hasbro.UIElements;
using System;
using UnityEngine.UI;
using Hasbro.TheGameOfLife.Shared;

namespace Hasbro.TheGameOfLife.GameSelection
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

        public PlayerType PlayerType => playerType;
        public CharacterColor CharacterColor => characterColor;

        internal void Init()
        {
            playerNumberText.text = $"Player {playerNumber}";

            int maxColors = Enum.GetValues(typeof(CharacterColor)).Length;
            colorSelection.Init(0, maxColors);

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
            playerNameText.text = $"{playerType} {playerNumber}";
            pawn.color = GameController.GetColor(characterColor);

            characterSelectionContainer.SetActive(playerType != PlayerType.None);
            joinContainer.SetActive(playerType == PlayerType.None);
        }
    }
}
