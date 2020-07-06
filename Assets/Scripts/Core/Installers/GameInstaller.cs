using System;
using Core.Misc;
using Core.Models;
using Core.Presenters;
using Core.Services;
using Core.Views.Game;
using Core.Views.UI;
using Photon.Pun;
using UnityEngine;

namespace Core
{
    public class GameInstaller : MonoBehaviour
    {
#pragma warning disable 0649
        [SerializeField] private GameConfig _config;

        [SerializeField] private Camera _camera;

        [Space] [SerializeField] private Trigger _leftTrigger;
        [SerializeField] private Trigger _righttTrigger;

        [Space] [SerializeField] private RacketView _leftRacket;
        [SerializeField] private RacketView _rightRacket;
        [SerializeField] private BallView _ball;

        [Space] [SerializeField] private InGameView _gameView;
#pragma warning restore 0649

        private IGameModel _gameModel;
        private PlayerInputService _inputService;
        private BallModel _ballModel;

        private void Awake()
        {
            _gameView.Hide();
        }

        public void Initialize(PlayerRole playerRole)
        {
            _leftRacket.SetStyle(_config.RacketPrefab);
            _rightRacket.SetStyle(_config.RacketPrefab);

            _ballModel = gameObject.AddComponent<BallModel>().Initialize(_ball);

            _gameModel = PhotonNetwork.IsMasterClient
                ? (IGameModel) new GameModel(_leftTrigger, _righttTrigger, _config)
                : (IGameModel) new ClientGameModel();

            var inGamePresenter = new InGamePresenter(_gameView, _ballModel, _gameModel, _config);

            if (playerRole == PlayerRole.Player)
            {
                var targetRacket = GetTargetRacket();
                _inputService = gameObject.AddComponent<PlayerInputService>().Initialize(_camera, targetRacket);
            }
            
            _gameModel.ResetScore();
            _ballModel.StartSimulation(_config.BallSpeed);
        }

        public void ResetGame()
        {
            _ballModel.StartSimulation();
        }

        private RacketView GetTargetRacket()
        {
            return PhotonNetwork.LocalPlayer.IsMasterClient ? _leftRacket : _rightRacket;
        }
    }
}