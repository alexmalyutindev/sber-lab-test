using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class ClientGameModel : IGameModel
    {
        public event Action<PlayerSide> OnPlayerWins;
        public event Action<int, int> OnScoreChanged;

        private Dictionary<PlayerSide, int> _scores;

        public ClientGameModel()
        {
            _scores = new Dictionary<PlayerSide, int> {{PlayerSide.Left, 0}, {PlayerSide.Right, 0}};
        }
        
        public void AddScore(PlayerSide side)
        {
            _scores[side]++;
            OnScoreChanged?.Invoke(_scores[PlayerSide.Left], _scores[PlayerSide.Right]);
        }

        public void SetPlayerWin(PlayerSide side)
        {
            
        }

        public void SetScores(int left, int right)
        {
            _scores[PlayerSide.Left] = left;
            _scores[PlayerSide.Right] = right;
            OnScoreChanged?.Invoke(left, right);
        }

        public void ResetScore()
        {
            _scores = new Dictionary<PlayerSide, int> {{PlayerSide.Left, 0}, {PlayerSide.Right, 0}};
            OnScoreChanged?.Invoke(_scores[PlayerSide.Left], _scores[PlayerSide.Right]);
        }
    }
}