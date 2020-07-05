using System;
using UnityEngine;
using UnityEngine.UI;

namespace Core.Views.UI
{
    public class RoomListItemView : MonoBehaviour
    {
        public Action OnPlay;
        public Action OnSpectate;
        
#pragma warning disable 0649
        [SerializeField] private Text _roomNameField;
        [SerializeField] private Button _join;
        [SerializeField] private Button _spectate;
#pragma warning restore 0649
        
        private void Awake()
        {
            _join.onClick.AddListener(() => OnPlay?.Invoke());
            _spectate.onClick.AddListener(() => OnSpectate?.Invoke());
        }

        public void SetRoomName(string roomName) => _roomNameField.text = roomName;
    }
}