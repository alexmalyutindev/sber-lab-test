using System;
using Core.Views.Game;
using UnityEngine;

namespace Core.Models
{
    public class BallModel : MonoBehaviour
    {
        private Rigidbody2D _ball;

        private RaycastHit2D[] _hits = new RaycastHit2D[1];
        
        public BallModel Initialize(BallView view)
        {
            _ball = view.Rigidbody;
            _ball.velocity = Vector2.right;
            return this;
        }

        private void FixedUpdate()
        {
            UpdatePosition(Time.fixedDeltaTime * 5);
        }
        
        public void UpdatePosition(float moveDelta)
        {
            var normalizedVelocity = _ball.velocity.normalized;

            var count = _ball.Cast(normalizedVelocity, _hits, moveDelta);

            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var reflectVelocity = Vector2.Reflect(normalizedVelocity, _hits[i].normal);
                    _ball.position += normalizedVelocity * _hits[i].distance + reflectVelocity * (moveDelta - _hits[i].distance);
                    _ball.velocity = reflectVelocity * moveDelta;
                }
            }
            else
            {
                _ball.MovePosition(_ball.position + normalizedVelocity * moveDelta);
            }
        }
    }
}