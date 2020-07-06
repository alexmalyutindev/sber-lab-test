using System;
using System.Collections.Generic;
using System.Linq;
using Core.Models;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Core.Services
{
    public class MatchMakingService : ILobbyCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
    {
        public event Action OnJoinedLobby;
        public event Action OnLeftLobby;
        public event Action<List<RoomInfo>> OnRoomListUpdate;
        public event Action<List<FriendInfo>> OnFriendListUpdate;
        public event Action OnGameStart;

        public event Action<List<Player>> OnPlayerListUpdated;

        public event Action<List<TypedLobbyInfo>> OnLobbyStatisticsUpdate;

        public event Action<PlayerRole> OnJoinedRoom;
        public event Action OnLeftRoom;

        public PlayerRole PlayerRole;
        private List<RoomInfo> _cachedRoomList;

        public void JoinRoom(RoomInfo room)
        {
            PhotonNetwork.JoinRoom(room.Name);
        }

        public void JoinRoomAs(RoomInfo room, PlayerRole role)
        {
            PlayerRole = role;
            PhotonNetwork.JoinRoom(room.Name);
        }

        public void CreateRoom(string roomName)
        {
            PlayerRole = PlayerRole.Player;
            PhotonNetwork.CreateRoom(roomName);
        }

        public void SetPlayerRole(PlayerRole role)
        {
            if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("PlayerRole"))
                PhotonNetwork.LocalPlayer.CustomProperties["PlayerRole"] = role;
            else
                PhotonNetwork.LocalPlayer.CustomProperties.Add("PlayerRole", role);
        }

        private void UpdatePlayerList()
        {
            OnPlayerListUpdated?.Invoke(PhotonNetwork.CurrentRoom.Players.Values.ToList());
        }

        void ILobbyCallbacks.OnJoinedLobby() => OnJoinedLobby?.Invoke();

        void ILobbyCallbacks.OnLeftLobby() => OnLeftLobby?.Invoke();

        void IMatchmakingCallbacks.OnJoinedRoom()
        {
            OnJoinedRoom?.Invoke(PlayerRole);
            UpdatePlayerList();
        }

        void IMatchmakingCallbacks.OnLeftRoom() => OnLeftRoom?.Invoke();

        void ILobbyCallbacks.OnRoomListUpdate(List<RoomInfo> roomList)
        {
            _cachedRoomList = roomList.Where(info => !info.RemovedFromList).ToList();
            OnRoomListUpdate?.Invoke(_cachedRoomList);
        }

        void ILobbyCallbacks.OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics) =>
            OnLobbyStatisticsUpdate?.Invoke(lobbyStatistics);

        void IMatchmakingCallbacks.OnFriendListUpdate(List<FriendInfo> friendList) =>
            OnFriendListUpdate?.Invoke(friendList);

        void IMatchmakingCallbacks.OnCreatedRoom()
        {
        }

        void IMatchmakingCallbacks.OnCreateRoomFailed(short returnCode, string message)
        {
        }

        void IMatchmakingCallbacks.OnJoinRoomFailed(short returnCode, string message)
        {
        }

        void IMatchmakingCallbacks.OnJoinRandomFailed(short returnCode, string message)
        {
        }


        void IInRoomCallbacks.OnPlayerEnteredRoom(Player newPlayer)
        {
            UpdatePlayerList();
        }

        void IInRoomCallbacks.OnPlayerLeftRoom(Player otherPlayer)
        {
            UpdatePlayerList();
        }

        void IInRoomCallbacks.OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
        }

        void IInRoomCallbacks.OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
        }

        void IInRoomCallbacks.OnMasterClientSwitched(Player newMasterClient)
        {
        }
    }
}