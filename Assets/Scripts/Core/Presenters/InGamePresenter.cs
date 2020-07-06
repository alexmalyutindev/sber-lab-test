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

            _view.Show(PhotonNetwork.IsMasterClient
                       || TryGetPlayerRole(out var role) && (PlayerRole) role == PlayerRole.Spectator
            );
            _view.Initialize(config.MinBallSpeed, config.MaxBallSpeed, config.BallSpeed);
            _view.OnSpeedSliderChanged += SpeedSliderChangedHandler;
            _view.OnSpeedChanged += ballModel.SetBallSpeed;

            _view.OnPlayerWins += playerSide => 
                _view.ShowMessage($"Wins {playerSide} player!\nRestart?", PhotonNetwork.IsMasterClient);

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

                    _view.ShowMessage($"Wins {side} player!\nRestart?", PhotonNetwork.IsMasterClient)
                        .ContinueWith(task =>
                        {
                            if (task.Result)
                            {
                                PhotonView.Get(_view).RPC("RestartGame", RpcTarget.Others);
                                gameModel.ResetScore();
                                ballModel.StartSimulation();
                            }
                        }, TaskScheduler.FromCurrentSynchronizationContext());
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

        private static bool TryGetPlayerRole(out object role)
        {
            return PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue("Role", out role);
        }

        private void SpeedSliderChangedHandler(float speed)
        {
            PhotonView.Get(_view).RPC("SetBallSpeed", RpcTarget.All, speed);
        }
    }
}