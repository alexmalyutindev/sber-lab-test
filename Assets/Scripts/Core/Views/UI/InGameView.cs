﻿using System;
using System.Threading.Tasks;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.UI
{
    public class InGameView : MonoBehaviour
    {
        public event Action<float> OnSpeedSliderChanged;
        public event Action<float> OnSpeedChanged;
        public event Action<int, int> OnScoresChanged;

        public event Action<string> OnPlayerWins;

#pragma warning disable 0649
        [Header("Ball Speed")] 
        [SerializeField] private GameObject _ballSpeedView;
        [SerializeField] private Slider _slider;
        [SerializeField] private Text _speedField;

        [Header("Scores")] 
        [SerializeField] private Text _leftScore;
        [SerializeField] private Text _rightScore;

        [Space] [SerializeField] private MessageWindow _messageWindowPreafb;
        private TaskCompletionSource<bool> _messageWindowTCS;
#pragma warning restore 0649

        private void Awake()
        {
            _slider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        private void SetSpeedText(float speed)
        {
            _speedField.text = $"Ball speed: {speed:F2}";
        }

        public void SetScores(int left, int right)
        {
            _leftScore.text = left.ToString();
            _rightScore.text = right.ToString();
        }

        public void Initialize(float minSpeed, float maxSpeed, float currentSpeed)
        {
            _slider.minValue = minSpeed;
            _slider.maxValue = maxSpeed;
            _slider.value = currentSpeed;

            SetSpeedText(currentSpeed);
        }

        private void OnSliderValueChanged(float speed)
        {
            OnSpeedSliderChanged?.Invoke(speed);
        }

        [PunRPC]
        public void SetBallSpeed(float speed)
        {
            SetSpeedText(speed);
            OnSpeedChanged?.Invoke(speed);
        }

        [PunRPC]
        public void ChangeScores(int left, int right)
        {
            OnScoresChanged?.Invoke(left, right);
        }

        public void Show(bool showBallSpeedView)
        {
            gameObject.SetActive(true);
            _ballSpeedView.SetActive(showBallSpeedView);
        }

        public async Task<bool> ShowMessage(string message, bool showButtons)
        {
            _messageWindowTCS = new TaskCompletionSource<bool>();
            return await Instantiate(_messageWindowPreafb, transform).Show(message, showButtons, _messageWindowTCS);
        }

        [PunRPC]
        public void SetPlayerWins(string playerSide)
        {
            OnPlayerWins?.Invoke(playerSide);
        }

        [PunRPC]
        public void RestartGame()
        {
            _messageWindowTCS?.SetCanceled();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}