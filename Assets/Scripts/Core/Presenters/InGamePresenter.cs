using Core.Models;
using Core.Views.UI;
using Photon.Pun;

namespace Core.Presenters
{
    public class InGamePresenter
    {
        private readonly InGameView _view;
        private readonly BallModel _ballModel;

        public InGamePresenter(InGameView view, BallModel ballModel, GameModel gameModel, GameConfig config,
            PlayerRole role)
        {
            _view = view;
            _ballModel = ballModel;

            _view.Show();
            _view.Initialize(config.MinBallSpeed, config.MaxBallSpeed, config.BallSpeed);
            _view.OnSpeedSliderChanged += SpeedSliderChangedHandler;
            _view.OnSpeedChanged += ballModel.SetBallSpeed;

            if (PhotonNetwork.IsMasterClient)
            {
                gameModel.OnScoreChanged += _view.SetScores;
                gameModel.OnScoreChanged += (_, __) => _ballModel.StartSimulation();
            }
        }

        private void SpeedSliderChangedHandler(float speed)
        {
            PhotonView.Get(_view).RPC("SetBallSpeed", RpcTarget.All, speed);
        }
    }
}