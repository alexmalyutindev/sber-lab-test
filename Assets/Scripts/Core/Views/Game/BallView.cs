using System;
using UnityEngine;

namespace Core.Views.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallView : MonoBehaviour
    {
        public Rigidbody2D Rigidbody => _rigidbody; 
        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetVelocity(Vector2 velocity)
        {
            _rigidbody.velocity = velocity;
        }
    }
}