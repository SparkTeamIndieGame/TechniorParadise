using Spark.Gameplay.Entities.Player;
using System;
using System.Collections;
using UnityEngine;

namespace Spark.Gameplay.Items.Pickupable.Doors
{
    public class AutoDoubleDoors : MonoBehaviour, IPickupable
    {
        [SerializeField] protected Transform _leftDoor;
        [SerializeField] protected Transform _rightDoor;
        [SerializeField, Range(0.5f, 3.0f)] protected float _moveTime = 1.2f;
        [SerializeField, Range(0.1f, 3.0f)] protected float _openOffset = 0.75f;

        protected bool _isActivated = false;
        protected bool _isUsed = false;

        void FixedUpdate()
        {
            if (_isUsed) return;
            if (_isActivated) OpenDoubleDoors();
        }

        protected void OpenDoubleDoors() => StartCoroutine(OpenDoorCoroutine());

        IEnumerator OpenDoorCoroutine()
        {
            float time = 0.0f;

            Vector3 leftInitialPosition = _leftDoor.position;
            Vector3 rightInitialPosition = _rightDoor.position;

            while (time <= _moveTime)
            {
                Vector3 leftTargetPosition = leftInitialPosition - (transform.right * _openOffset);
                Vector3 rightTargetPosition = rightInitialPosition + (transform.right * _openOffset);

                _leftDoor.position = Vector3.Lerp(leftInitialPosition, leftTargetPosition, time / _moveTime);
                _rightDoor.position = Vector3.Lerp(rightInitialPosition, rightTargetPosition, time / _moveTime);

                time += Time.fixedDeltaTime;
                yield return null;
            }
            _isUsed = true;
        }

        public virtual void Activate(PlayerModel player) => _isActivated = true;
    }
}