using System.Linq;
using Core.Services;
using Photon.Pun;

namespace Core.Presenters
{
    public class MatchMakingPresenter
    {
        private readonly LobbyView _lobbyView;
        private readonly RoomView _roomView;
        private readonly MatchMakingService _service;

        public MatchMakingPresenter(LobbyView lobbyView, RoomView roomView, MatchMakingService service)
        {
            _lobbyView = lobbyView;
            _roomView = roomView;
            _service = service;
            
            _lobbyView.gameObject.SetActive(false);
            _roomView.gameObject.SetActive(false);
            
            _service.OnRoomListUpdate += _lobbyView.UpdateRoomList;
            _service.OnLobbyStatisticsUpdate += statistics => 
                _roomView.SetPlayersList(
                    PhotonNetwork.CurrentRoom.Players.Values.Select(player => player.ToString()).ToList()
                );
            
            _service.OnJoinedLobby += () => _lobbyView.gameObject.SetActive(true);
            _service.OnLeftLobby += () => _lobbyView.gameObject.SetActive(false);
            
            _service.OnJoinedRoom += () =>
            {
                _roomView.gameObject.SetActive(true);
                _roomView.SetPlayersList(
                    PhotonNetwork.CurrentRoom.Players.Values.Select(player => $"Player{player.ActorNumber}").ToList()
                );
            };
            _service.OnLeftRoom += () => _roomView.gameObject.SetActive(false);
            
            
            
            _lobbyView.OnRoonJoin += _service.JoinRoom;
            _lobbyView.OnRoomCreate += _service.CreateRoom;
            
            
        }
    }
}