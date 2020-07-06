using System;

namespace Core.Models
{
    public interface IGameModel
    {
        event Action<PlayerSide> OnPlayerWins;
        event Action<int, int> OnScoreChanged;
        
        void ResetScore();
        void SetPlayerWin(PlayerSide side);
        void SetScores(int left, int right);
    }
}