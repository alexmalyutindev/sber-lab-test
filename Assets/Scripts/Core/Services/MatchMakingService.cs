using System;
using System.Collections.Generic;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;

namespace Core.Services
{
    public class MatchMakingService : ILobbyCallbacks, IMatchmakingCallbacks
    {
        public Action OnJoinedLobby;
        public Action OnLeftLobby;
        public Action<List<RoomInfo>> OnRoomListUpdate;
        public Action<List<FriendInfo>> OnFriendListUpdate;

        public Action<List<TypedLobbyInfo>> OnLobbyStatisticsUpdate;
 
        public Action OnJoinedRoom;
        public Action OnLeftRoom;


        public void JoinRoom(RoomInfo room) =>
            PhotonNetwork.JoinRoom(room.Name);

        public void CreateRoom(string roomName) =>
            PhotonNetwork.CreateRoom(roomName);

        void ILobbyCallbacks.OnJoinedLobby() => OnJoinedLobby?.Invoke();

        void ILobbyCallbacks.OnLeftLobby() => OnLeftLobby?.Invoke();

        void IMatchmakingCallbacks.OnJoinedRoom() => OnJoinedRoom?.Invoke();

        void IMatchmakingCallbacks.OnLeftRoom() => OnLeftRoom?.Invoke();
        
        void ILobbyCallbacks.OnRoomListUpdate(List<RoomInfo> roomList) => OnRoomListUpdate?.Invoke(roomList);



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
    }
}