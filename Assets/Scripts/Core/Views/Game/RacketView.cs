using Photon.Pun;
using UnityEngine;

namespace Core.Views.Game
{
    [RequireComponent(typeof(Rigidbody2D), typeof(PhotonView))]
    public class RacketView : MonoBehaviour
    {
        private Rigidbody2D _rigidbody;
        private PhotonView _photonView;

        public PhotonView NetworkView => 
            _photonView != null ? 
            _photonView :
            _photonView = GetComponent<PhotonView>();

        public void SetStyle(GameObject racketPrefab)
        {
            Instantiate(racketPrefab, transform);
        }

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public void SetPosition(Vector3 position)
        {
            var clampedPosition = new Vector2(_rigidbody.position.x, position.y);
            _rigidbody.MovePosition(clampedPosition);
        }
    }
}