using Spark.Gameplay.Entities.Player;
using Spark.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Spark.Utilities.Cameras
{ 
    public class FollowCameraPoint : MonoBehaviour
    {
        [SerializeField] private TurnableFollowCamera _turnableFollowCamera;

        void Start()
        {
            if (_turnableFollowCamera == null) _turnableFollowCamera = Camera.main.transform.GetComponent<TurnableFollowCamera>();
        }

        [SerializeField] private bool _toggleLookAtPlayerCameraBehaviour = false;

        private void OnTriggerEnter(Collider other)
        {
            _turnableFollowCamera.ToggleLookAtPlayerCameraBehaviour();
        }

        private void OnTriggerExit(Collider other)
        {            
            if (other.TryGetComponent<PlayerController>(out var player))
            {
                Vector3 currentPlayerPosition = other.transform.position;
                Vector3 direction = currentPlayerPosition - transform.position;

                var movementDirection = CalculateMovementDirectionSettingFrom(direction);
                
                player.MovementDirection = movementDirection;
                _turnableFollowCamera.MovementDirection = movementDirection;

                Debug.Log(movementDirection.ToString());
                _turnableFollowCamera.ToggleLookAtPlayerCameraBehaviour();
            }
        }

        private PlayerController.MovementDirectionSetting CalculateMovementDirectionSettingFrom(Vector3 direction)
        {
            direction.Normalize();
            float angle = Vector3.Angle(Vector3.forward, direction);

            if (angle <= 45.0f) return PlayerController.MovementDirectionSetting.Down;
            else if (angle <= 135)
            {
                if (direction.x < 0) return PlayerController.MovementDirectionSetting.Left;
                else return PlayerController.MovementDirectionSetting.Right;
            }
            else return PlayerController.MovementDirectionSetting.Up;
        }
    }
}