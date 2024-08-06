using System;
using UnityEngine;

namespace Spark.Utilities
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform _player;
        Vector3 _offset;

        private void Start() => _offset = transform.position - _player.position;

        void LateUpdate() => transform.position = _player.transform.position + _offset;
    }
}