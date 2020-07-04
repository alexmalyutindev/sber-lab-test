using System;
using System.Collections.Generic;
using System.Linq;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Core.Services
{
    public class MatchMakingService : ILobbyCallbacks, IMatchmakingCallbacks, IInRoomCallbacks
    {
        public Action OnJoinedLobby;
        public Action OnLeftLobby;
        public Action<List<RoomInfo>> OnRoomListUpdate;
        public Action<List<FriendInfo>> OnFriendListUpdate;

        public Action<List<Player>> OnPlayerListUpdated;

        public Action<List<TypedLobbyInfo>> OnLobbyStatisticsUpdate;
 
        public Action OnJoinedRoom;
        public Action OnLeftRoom;
        
        
        private List<RoomInfo> _cachedRoomList;

        public void JoinRoom(RoomInfo room) =>
            PhotonNetwork.JoinRoom(room.Name);

        public void CreateRoom(string roomName) =>
            PhotonNetwork.CreateRoom(roomName);

        private void UpdatePlayerList()
        {
            OnPlayerListUpdated?.Invoke(PhotonNetwork.CurrentRoom.Players.Values.ToList());
        }
        
        void ILobbyCallbacks.OnJoinedLobby() => OnJoinedLobby?.Invoke();

        void ILobbyCallbacks.OnLeftLobby() => OnLeftLobby?.Invoke();

        void IMatchmakingCallbacks.OnJoinedRoom()
        {
            OnJoinedRoom?.Invoke();
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