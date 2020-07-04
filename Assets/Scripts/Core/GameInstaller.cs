using Core.Models;
using Core.Services;
using Core.Views.Game;
using Photon.Pun;
using UnityEngine;

namespace Core
{
    public class GameInstaller : MonoBehaviour
    {
        [SerializeField] private GameConfig _config;

        [SerializeField] private Camera _camera;

        [Space] [SerializeField] private Trigger _leftTrigger;
        [SerializeField] private Trigger _righttTrigger;

        [Space] [SerializeField] private RacketView _leftRacket;
        [SerializeField] private RacketView _rightRacket;
        [SerializeField] private BallView _ball;

        private GameModel _gameModel;
        private PlayerInputService _inputService;

        public void Initialize()
        {
            _leftRacket.SetView(_config.RacketPrefab);
            _rightRacket.SetView(_config.RacketPrefab);

            if (PhotonNetwork.IsMasterClient)
                gameObject.AddComponent<BallModel>().Initialize(_ball);
            
            if (IsActivePlayer())
            {
                var targetRacket = GetTargetRacket();
                _inputService = gameObject.AddComponent<PlayerInputService>().Initialize(_camera, targetRacket);
            }
        }

        private static bool IsActivePlayer()
        {
            return PhotonNetwork.LocalPlayer.ActorNumber <= 2;
        }

        private RacketView GetTargetRacket()
        {
            switch (PhotonNetwork.LocalPlayer.ActorNumber)
            {
                case 1: return _leftRacket;
                case 2: return _rightRacket;
            }

            return null;
        }
    }
}