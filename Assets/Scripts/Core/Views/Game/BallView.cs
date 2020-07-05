using System;
using UnityEngine;

namespace Core.Views.Game
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class BallView : MonoBehaviour
    {
        public Rigidbody2D Rigidbody => 
            _rigidbody != null ? 
            _rigidbody : 
            _rigidbody = GetComponent<Rigidbody2D>(); 
        
        private Rigidbody2D _rigidbody;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(Rigidbody.position, Rigidbody.position + Rigidbody.velocity);
        }
    }
}