using System.Linq;
using Core.Models;
using Core.Services;
using Core.Views.UI;
using Photon.Pun;
using UnityEngine;

namespace Core.Presenters
{
    public class MatchMakingPresenter
    {
        private readonly LobbyView _lobbyView;
        private readonly RoomView _roomView;
        private readonly MatchMakingService _service;

        public MatchMakingPresenter(LobbyView lobbyView, RoomView roomView, MatchMakingService service,
            GameInstaller gameInstaller)
        {
            _lobbyView = lobbyView;
            _roomView = roomView;
            _service = service;
            
            _lobbyView.Hide();
            _roomView.Hide();
            
            _service.OnRoomListUpdate += _lobbyView.UpdateRoomList;
            _service.OnPlayerListUpdated += players => 
                _roomView.SetPlayersList(players.Select(player => $"Player{player.ActorNumber}").ToList());
            
            _service.OnJoinedLobby += () => _lobbyView.Show();
            _service.OnLeftLobby += () => _lobbyView.Hide();
            
            _service.OnJoinedRoom += role =>
            {
                if (role == PlayerRole.Player)
                    _roomView.Show(PhotonNetwork.CurrentRoom.Name, PhotonNetwork.IsMasterClient);
                _lobbyView.Hide();
            };
            
            _service.OnLeftRoom += () =>
            {
                if(_roomView != null) _roomView.gameObject.SetActive(false);
                if(_lobbyView != null) _lobbyView.gameObject.SetActive(true);
            };

            _lobbyView.OnRoomCreate += _service.CreateRoom;
            _lobbyView.OnJoinRoom += room => _service.JoinRoomAs(room, PlayerRole.Player);
            _lobbyView.OnSpecatateRoom += room =>
            {
                _service.JoinRoomAs(room, PlayerRole.Spectator);
                //PhotonView.Get(roomView).RPC("StartGame", RpcTarget.All);
                gameInstaller.Initialize(_service.PlayerRole);
            };
            
            _roomView.OnRequestPlay += () =>
            {
                // TODO: Преместить RPC в одну шину 
                PhotonView.Get(roomView).RPC("StartGame", RpcTarget.All);
            };

            _roomView.OnStartGame += () =>
            {
                _lobbyView.gameObject.SetActive(false);
                _roomView.gameObject.SetActive(false);
                gameInstaller.Initialize(_service.PlayerRole);
            };
        }
    }
}