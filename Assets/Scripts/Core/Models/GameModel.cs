using UnityEngine;

namespace Core
{
    public class GameModel
    {
        private Rigidbody _leftRacket;
        private Rigidbody _rightRacket;
        private Rigidbody _ball;
        
        public GameModel(Rigidbody leftRacket, Rigidbody rightRacket, Rigidbody ball)
        {
            _leftRacket = leftRacket;
            _rightRacket = rightRacket;
            _ball = ball;
        }

        public void Update()
        {
            
        }
    }
}