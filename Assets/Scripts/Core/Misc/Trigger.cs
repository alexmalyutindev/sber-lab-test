using System;
using UnityEngine;

namespace Core.Misc
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Trigger : MonoBehaviour
    {
        public event Action OnTriggerEnter;

        [SerializeField] private LayerMask _mask;

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log($"-> Trigger {other}!", this);
            if ((1 << other.gameObject.layer & _mask.value) > 0)
            {
                OnTriggerEnter?.Invoke();
            }
        }
    }
}