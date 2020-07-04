using System;
using UnityEngine;

namespace Core
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class RacketView : MonoBehaviour
    {
        private Rigidbody2D _rigidbody2D;
        
        public void SetView(GameObject racketPrefab)
        {
            Instantiate(racketPrefab, transform);
        }

        private void Awake()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
        }

        public void SetPosition(Vector3 position)
        {
            var clampedPosition = new Vector2(_rigidbody2D.position.x, position.y);
            _rigidbody2D.MovePosition(clampedPosition);
        }
    }
}