using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class LobbyView : MonoBehaviour
{
    public Action<RoomInfo> OnRoonJoin;
    public Action<string> OnRoomCreate;
    
    [SerializeField] private ButtonWithTextView _itemPrefab;
    [SerializeField] private Transform _itemsRoot;
    [SerializeField] private Button _createRoom;
    [SerializeField] private InputField _roomNameField;

    private Dictionary<RoomInfo, ButtonWithTextView> _views = new Dictionary<RoomInfo, ButtonWithTextView>();
    private List<RoomInfo> _rooms = new List<RoomInfo>();

    private void Awake()
    {
        _createRoom.onClick.AddListener(OnCreatedRoomHandler);
    }

    private void OnCreatedRoomHandler()
    {
        OnRoomCreate?.Invoke(_roomNameField.text);
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

    private void RemoveRoomItem(RoomInfo roomInfo)
    {
        Destroy(_views[roomInfo]);
        _views.Remove(roomInfo);
    }

    private ButtonWithTextView CreateRoomItem(RoomInfo roomInfo)
    {
        var item = Instantiate(_itemPrefab, _itemsRoot);
        item.SetText(roomInfo.Name);
        item.OnClick += () => OnRoonJoin?.Invoke(roomInfo);
        return item;
    }
}