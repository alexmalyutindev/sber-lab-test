using UnityEngine;

namespace Core
{
    [CreateAssetMenu()]
    public class GameConfig : ScriptableObject
    {
        public GameObject RacketPrefab;
        public int MaxScores = 10;
        public float BallSpeed;
    }
}