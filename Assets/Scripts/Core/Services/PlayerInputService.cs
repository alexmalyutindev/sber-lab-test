using System;
using Core.Views.Game;
using UnityEngine;

namespace Core.Services
{
    public class PlayerInputService : MonoBehaviour
    {
        private Camera _camera;
        private RacketView _view;
        
        public PlayerInputService Initialize(Camera camera, RacketView view)
        {
            _camera = camera;
            _view = view;
            _view.NetworkView.RequestOwnership();
            return this;
        }

        private void FixedUpdate()
        {
            if (Input.GetMouseButton(0))
            {
                var position = _camera.ScreenToWorldPoint(Input.mousePosition);
                _view.SetPosition(position);
            }
        }
    }
}