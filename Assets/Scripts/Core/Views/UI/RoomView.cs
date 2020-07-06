using System;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.UI
{
    public class RoomView : MonoBehaviour
    {
        public event Action OnRequestPlay;
        public event Action OnStartGame;
        
#pragma warning disable 0649
        [SerializeField] private Text _roomNameField;
        [SerializeField] private Text _playerListField;
        [SerializeField] private Button _play;
#pragma warning restore 0649
        
        private void Awake()
        {
            _play.onClick.AddListener(PlayHandler);
        }

        public void Show(string roomName, bool playButtonActive)
        {
            gameObject.SetActive(true);
            _roomNameField.text = $"Room: {roomName}";
            _play.interactable = playButtonActive;
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void PlayHandler() => OnRequestPlay?.Invoke();

        [PunRPC]
        public void StartGame()
        {
            OnStartGame?.Invoke();
        }

        public void SetPlayersList(List<string> players) => 
            _playerListField.text = String.Join("\n", players);


        public void SetPlayButtonActive(bool active)
        {
            _play.interactable = active;
        }
    }
}