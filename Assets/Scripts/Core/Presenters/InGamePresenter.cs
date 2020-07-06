using System.Threading.Tasks;
using Core.Models;
using Core.Views.UI;
using Photon.Pun;

namespace Core.Presenters
{
    public class InGamePresenter
    {
        private readonly InGameView _view;

        public InGamePresenter(InGameView view, BallModel ballModel, IGameModel gameModel, GameConfig config)
        {
            _view = view;

            _view.Show();
            _view.Initialize(config.MinBallSpeed, config.MaxBallSpeed, config.BallSpeed);
            _view.OnSpeedSliderChanged += SpeedSliderChangedHandler;
            _view.OnSpeedChanged += ballModel.SetBallSpeed;

            _view.OnPlayerWins += playerSide =>
            {
                _view.ShowMessage($"Wins {playerSide} player!\nRestart?");
            };

            if (PhotonNetwork.IsMasterClient)
            {
                gameModel.OnScoreChanged += (left, right) =>
                {
                    view.SetScores(left, right);
                    ballModel.StartSimulation();
                    PhotonView.Get(_view).RPC("ChangeScores", RpcTarget.Others, left, right);
                };

                gameModel.OnPlayerWins += side =>
                {
                    ballModel.StopSimulation();
                    PhotonView.Get(_view).RPC("SetPlayerWins", RpcTarget.Others, side.ToString());
                    
                    _view.ShowMessage($"Wins {side} player!\nRestart?")
                        .ContinueWith(task =>
                        {
                            if (task.Result)
                            {
                                gameModel.ResetScore();
                                ballModel.StartSimulation(config.BallSpeed);
                            }
                        },TaskScheduler.FromCurrentSynchronizationContext());
                };
            }
            else
            {
                _view.OnScoresChanged += (left, right) =>
                {
                    view.SetScores(left, right);
                    gameModel.SetScores(left, right);
                };
            }
        }

        private void SpeedSliderChangedHandler(float speed)
        {
            PhotonView.Get(_view).RPC("SetBallSpeed", RpcTarget.All, speed);
        }
    }
}