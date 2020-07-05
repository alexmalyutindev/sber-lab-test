using UnityEngine;

namespace Core
{
    [CreateAssetMenu()]
    public class GameConfig : ScriptableObject
    {
        public GameObject RacketPrefab;
        public int MaxScore = 10;
        public float BallSpeed = 3f;
        public float MinBallSpeed = 0.1f;
        public float MaxBallSpeed = 5f;
    }
}