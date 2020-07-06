using System;
using System.Collections.Generic;
using Core.Misc;
using UnityEngine;

namespace Core.Models
{
    public class GameModel : IGameModel
    {
        public event Action<PlayerSide> OnPlayerWins;
        public event Action<int, int> OnScoreChanged;

        private Dictionary<PlayerSide, int> _scores;

        private readonly GameConfig _config;

        public GameModel(Trigger leftPlayerTrigger, Trigger rightPlayerTrigger, GameConfig config)
        {
            _config = config;

            _scores = new Dictionary<PlayerSide, int>() {{PlayerSide.Left, 0}, {PlayerSide.Right, 0}};

            leftPlayerTrigger.OnTriggerEnter += () => AddScore(PlayerSide.Right);
            rightPlayerTrigger.OnTriggerEnter += () => AddScore(PlayerSide.Left);
        }

        public void ResetScore()
        {
            _scores = new Dictionary<PlayerSide, int>() {{PlayerSide.Left, 0}, {PlayerSide.Right, 0}};
            OnScoreChanged?.Invoke(_scores[PlayerSide.Left], _scores[PlayerSide.Right]);
        }

        public void SetPlayerWin(PlayerSide side)
        {
            OnPlayerWins?.Invoke(side);
        }

        public void SetScores(int left, int right) { }

        private void AddScore(PlayerSide side)
        {
            _scores[side]++;
            OnScoreChanged?.Invoke(_scores[PlayerSide.Left], _scores[PlayerSide.Right]);
            if (_scores[side] >= _config.MaxScore)
                SetPlayerWin(side);
        }
    }

    public enum PlayerRole
    {
        Player = 0,
        Spectator = 1
    }

    public enum PlayerSide
    {
        Left,
        Right
    }
}