using Core;
using Core.Presenters;
using Core.Services;
using Photon.Pun;
using UnityEngine;

public class LobbyInstaller : MonoBehaviour
{
    [SerializeField] private GameInstaller _gameInstaller;

    [Space]
    [SerializeField] private LobbyView _lobbyView;
    [SerializeField] private RoomView _roomView;
     
    private ConnectionService _connectionService;
    private MatchMakingService _matchMakingService;
    
    private MatchMakingPresenter _lobbyPresenter;

    private void Awake() => Initialize();

    private void Initialize()
    {
        _connectionService = new ConnectionService();
        _matchMakingService = new MatchMakingService();
        PhotonNetwork.AddCallbackTarget(_connectionService);
        PhotonNetwork.AddCallbackTarget(_matchMakingService);

        _lobbyPresenter = new MatchMakingPresenter(_lobbyView, _roomView, _matchMakingService, _gameInstaller);

        _connectionService.ConnectAndJoinLobby();
    }
}
