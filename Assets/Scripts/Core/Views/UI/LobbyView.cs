using System;
using System.Collections.Generic;
using System.Linq;
using Core.Views.UI;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    public event Action<RoomInfo> OnJoinRoom;
    public event Action<RoomInfo> OnSpecatateRoom;
    public event Action<string> OnRoomCreate;

#pragma warning disable 0649
    [SerializeField] private RoomListItemView _itemPrefab;
    [SerializeField] private Transform _itemsRoot;
    [SerializeField] private Button _createRoom;
    [SerializeField] private InputField _roomNameField;
#pragma warning restore 0649
    
    private Dictionary<RoomInfo, RoomListItemView> _views = new Dictionary<RoomInfo, RoomListItemView>();
    private List<RoomInfo> _rooms = new List<RoomInfo>();

    private void Awake()
    {
        _createRoom.onClick.AddListener(OnCreatedRoomHandler);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void UpdateRoomList(List<RoomInfo> roomList)
    {
        var deleted = _rooms.Except(roomList).ToList();
        var added = roomList.Except(_rooms).ToList();

        foreach (var roomInfo in deleted)
            RemoveRoomItem(roomInfo);

        foreach (var roomInfo in added)
            _views.Add(roomInfo, CreateRoomItem(roomInfo));

        _rooms = roomList;
    }

    private void OnCreatedRoomHandler()
    {
        OnRoomCreate?.Invoke(_roomNameField.text);
    }

    private void RemoveRoomItem(RoomInfo roomInfo)
    {
        Destroy(_views[roomInfo]);
        _views.Remove(roomInfo);
    }

    private RoomListItemView CreateRoomItem(RoomInfo roomInfo)
    {
        var item = Instantiate(_itemPrefab, _itemsRoot);
        item.SetRoomName(roomInfo.Name);
        item.OnPlay += () => OnJoinRoom?.Invoke(roomInfo);
        item.OnSpectate += () => OnSpecatateRoom?.Invoke(roomInfo);
        return item;
    }
}