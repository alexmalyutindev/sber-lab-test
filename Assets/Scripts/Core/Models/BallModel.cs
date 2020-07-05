using System;
using Core.Views.Game;
using Photon.Pun;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Core.Models
{
    public class BallModel : MonoBehaviour
    {
        private Rigidbody2D _ball;

        private RaycastHit2D[] _hits = new RaycastHit2D[1];

        private ContactFilter2D _contactFilter = new ContactFilter2D()
        {
            useTriggers = false
        };

        private bool _simulate = false;
        public float BallSpeed => _ballSpeed;
        private float _ballSpeed;

        public BallModel Initialize(BallView view)
        {
            _ball = view.Rigidbody;
            return this;
        }

        public void StartSimulation(float initialBallSpeed)
        {
            _ballSpeed = initialBallSpeed;
            _ball.position = Vector2.zero;
            var side = Random.value > 0.5;
            var direction = (new Vector2(side ? -1 : 1, Random.Range(-0.5f, 0.5f))).normalized;
            _ball.velocity = direction * initialBallSpeed * Time.fixedDeltaTime;
            _simulate = true;
        }
        
        public void StartSimulation()
        {
            StartSimulation(_ballSpeed);
        }

        public void StopSimulation()
        {
            _ball.position = Vector2.zero;
            _ball.velocity = Vector2.zero;
            _simulate = false;
        }

        [PunRPC]
        public void SetBallSpeed(float speed)
        {
            _ballSpeed = speed;
        }

        private void FixedUpdate()
        {
            if (_simulate)
                UpdatePosition(Time.fixedDeltaTime * _ballSpeed);
        }

        public void UpdatePosition(float moveDelta)
        {
            var normalizedVelocity = _ball.velocity.normalized;

            var count = _ball.Cast(normalizedVelocity, _contactFilter, _hits, moveDelta + 0.02f);
            if (count > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    Debug.DrawLine(_ball.position, _ball.position + normalizedVelocity * _hits[i].distance, Color.green,
                        10);

                    var reflectVelocity = Vector2.Reflect(normalizedVelocity, _hits[i].normal);
                    _ball.position += normalizedVelocity * _hits[i].distance + 
                        reflectVelocity * Mathf.Max(_hits[i].distance, moveDelta - _hits[i].distance);
                    
                    _ball.velocity = reflectVelocity * moveDelta;
                }
            }
            else
            {
                _ball.MovePosition(_ball.position + normalizedVelocity * moveDelta);
                _ball.velocity = normalizedVelocity * moveDelta;
            }
        }


    }
}