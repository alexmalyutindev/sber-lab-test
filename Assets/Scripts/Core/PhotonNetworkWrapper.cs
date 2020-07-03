using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Core
{
    public class PhotonNetworkWrapper : IConnectionCallbacks, IMatchmakingCallbacks
    {
        private Action OnConnected;
        private Action OnConnectedToMaster;
        private Action<Room> OnCreatedRoom;

        public async Task ConnectUsingSettings()
        {
            PhotonNetwork.ConnectUsingSettings();
            var tcs = new TaskCompletionSource<bool>();

            void OnCompleted()
            {
                Debug.Log($"Complete OnConnected.");
                tcs.SetResult(true);
            }

            OnConnectedToMaster += OnCompleted;
            await tcs.Task.ContinueWith(_ => OnConnected -= OnCompleted);
        }

        public async Task CreateRoom(string roomName, RoomOptions roomOptions)
        {
            var tcs = new TaskCompletionSource<bool>();
            void OnCompleted(object _) => tcs.SetResult(true);
            OnCreatedRoom += OnCompleted;

            PhotonNetwork.CreateRoom(roomName, roomOptions);

            await tcs.Task.ContinueWith(_ => OnCreatedRoom -= OnCompleted);
        }

        private async Task AwaitEvent(Action @event)
        {
            var tcs = new TaskCompletionSource<bool>();
            Action onCompleted = () =>
            {
                tcs.SetResult(true);
                Debug.Log($"Complete {@event.Method.Name}");
            };
            @event += onCompleted;
            await tcs.Task.ContinueWith(_ => @event -= onCompleted);
        }

        // IConnectionCallbacks
        void IConnectionCallbacks.OnConnected()
        {
            Debug.Log("IConnectionCallbacks.OnConnected");
            OnConnected?.Invoke();
        }

        void IConnectionCallbacks.OnConnectedToMaster()
        {
            Debug.Log("IConnectionCallbacks.OnConnectedToMaster");
            OnConnectedToMaster?.Invoke();
        }

        void IConnectionCallbacks.OnDisconnected(DisconnectCause cause)
        {
            Debug.Log($"IConnectionCallbacks.OnDisconnected {cause}");
            // throw new System.NotImplementedException();
        }

        void IConnectionCallbacks.OnRegionListReceived(RegionHandler regionHandler)
        {
            // throw new System.NotImplementedException();
        }

        void IConnectionCallbacks.OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
            throw new System.NotImplementedException();
        }

        void IConnectionCallbacks.OnCustomAuthenticationFailed(string debugMessage)
        {
            throw new System.NotImplementedException();
        }

        // IMatchmakingCallbacks
        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
            throw new NotImplementedException();
        }

        void IMatchmakingCallbacks.OnCreatedRoom()
        {
            Debug.Log("IMatchmakingCallbacks.OnCreatedRoom");
            OnCreatedRoom?.Invoke(PhotonNetwork.CurrentRoom);
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
            throw new NotImplementedException();
        }

        void IMatchmakingCallbacks.OnJoinedRoom()
        {
            Debug.Log("IMatchmakingCallbacks.OnJoinedRoom");
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
            throw new NotImplementedException();
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            throw new NotImplementedException();
        }

        public void OnLeftRoom()
        {
            throw new NotImplementedException();
        }
    }
}