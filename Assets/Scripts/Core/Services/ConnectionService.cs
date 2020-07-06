using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using Random = System.Random;

namespace Core.Services
{
    public class ConnectionService : IConnectionCallbacks
    {
        public void ConnectAndJoinLobby()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        public void OnConnected() { }

        public void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
        }

        public void OnDisconnected(DisconnectCause cause) { }
        public void OnRegionListReceived(RegionHandler regionHandler) { }
        public void OnCustomAuthenticationResponse(Dictionary<string, object> data) { }
        public void OnCustomAuthenticationFailed(string debugMessage) { }
    }
}