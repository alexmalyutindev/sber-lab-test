using System.Collections.Generic;
using System.Linq;
using Core.Presenters;
using Core.Services;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class LobbyInstaller : MonoBehaviour
{
    [SerializeField] private LobbyView _lobbyView;
    [SerializeField] private RoomView _roomView;
     
    private ConnectionService _connectionService;
    private MatchMakingService _matchMakingService;
    
    private MatchMakingPresenter _lobbyPresenter;

    private void Awake()
    {
        _connectionService = new ConnectionService();
        _matchMakingService = new MatchMakingService();
        PhotonNetwork.AddCallbackTarget(_connectionService);
        PhotonNetwork.AddCallbackTarget(_matchMakingService);

        _lobbyPresenter = new MatchMakingPresenter(_lobbyView, _roomView, _matchMakingService);

        _connectionService.ConnectAndJoinLobby();
    }
}
