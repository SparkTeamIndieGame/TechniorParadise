using Spark.Gameplay.Entities.Player;
using System;
using UnityEngine;

namespace Spark.Utilities.Cameras
{
    public class TurnableFollowCamera : MonoBehaviour
    {
        [SerializeField] Transform _player;

        [SerializeField] Vector3 _cameraPositionLookAtPlayer = new Vector3(7.41f, 15.0f, 79.52f);

        [SerializeField] Vector3 _offsetToUp = new Vector3(0.0f, 15.0f, -10.0f);
        [SerializeField] Vector3 _offsetToRight = new Vector3(0.0f, 15.0f, -10.0f);

        [SerializeField] Vector3 _cameraRotationToUp = new Vector3 (70.0f, 270.0f, 0.0f);
        [SerializeField] Vector3 _cameraRotationToRight = new Vector3 (80.0f, .0f, 0.0f);

        [SerializeField] private PlayerController.MovementDirectionSetting _movementDirectionSetting;        
        Vector3 _offset = new Vector3(0.0f, 15.0f, -10.0f);

        private void Start()
        {
            _offset = _offsetToUp;
        }

        public PlayerController.MovementDirectionSetting MovementDirection 
        {
            get => _movementDirectionSetting;
            set
            {
                _movementDirectionSetting = value;
                UpdateOffsetFromMovementDirectionSetting(_movementDirectionSetting);
            }
        }

        void UpdateOffsetFromMovementDirectionSetting(PlayerController.MovementDirectionSetting setting)
        {
            if (setting == PlayerController.MovementDirectionSetting.Up)
            {
                _offset = _offsetToUp;
                Camera.main.transform.rotation = Quaternion.Euler(_cameraRotationToUp);
            }

            else if (setting == PlayerController.MovementDirectionSetting.Right)
            {
                _offset = _offsetToRight;
                Camera.main.transform.rotation = Quaternion.Euler(_cameraRotationToRight);
            }
        }

        [SerializeField] private bool _toggleLookAtPlayerCameraBehaviour = false;
        public void ToggleLookAtPlayerCameraBehaviour()
        {
            _toggleLookAtPlayerCameraBehaviour = !_toggleLookAtPlayerCameraBehaviour;
        }
        void LateUpdate()
        {
            if (!_toggleLookAtPlayerCameraBehaviour)
                transform.position = _player.transform.position + _offset;

            else
            {
                transform.position = _cameraPositionLookAtPlayer;
                transform.LookAt(_player);
            }
        }

        public void Zoom(float zooming) => _offset.y -= zooming;
    }
}