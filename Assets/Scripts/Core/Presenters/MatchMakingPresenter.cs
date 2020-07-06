using System.Linq;
using Core.Models;
using Core.Services;
using Core.Views.UI;
using ExitGames.Client.Photon;
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
            {
                _roomView.SetPlayButtonActive(
                    PhotonNetwork.IsMasterClient
                    && PhotonNetwork.CurrentRoom.Players
                        .Count(player =>
                            player.Value.CustomProperties.TryGetValue("Role", out var role)
                            && (PlayerRole) role == PlayerRole.Player
                        ) > 1
                ); // Проверка на количесиво активных игроков в комнате

                _roomView.SetPlayersList(players.Select(player => $"Player{player.ActorNumber}").ToList());
            };
            _service.OnJoinedLobby += () => _lobbyView.Show();
            _service.OnLeftLobby += () => _lobbyView.Hide();

            _service.OnJoinedRoom += role =>
            {
                if (role == PlayerRole.Player)
                    _roomView.Show(PhotonNetwork.CurrentRoom.Name, false);
                _lobbyView.Hide();
            };

            _service.OnLeftRoom += () =>
            {
                if (_roomView != null) _roomView.gameObject.SetActive(false);
                if (_lobbyView != null) _lobbyView.gameObject.SetActive(true);
            };

            _lobbyView.OnCreateRoom += roomName =>
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"Role", PlayerRole.Player}});
                _service.CreateRoom(roomName);
            };
            _lobbyView.OnJoinRoom += room =>
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"Role", PlayerRole.Player}});
                _service.JoinRoomAs(room, PlayerRole.Player);
            };
            _lobbyView.OnSpecatateRoom += room =>
            {
                PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable {{"Role", PlayerRole.Spectator}});
                _service.JoinRoomAs(room, PlayerRole.Spectator);
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