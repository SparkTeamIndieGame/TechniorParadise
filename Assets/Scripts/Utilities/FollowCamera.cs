using System;
using UnityEngine;

namespace Spark.Utilities
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform _player;
        [SerializeField] Vector3 _offset = new Vector3(0.0f, 15.0f, -10.0f);

        //void LateUpdate() => transform.position = _player.transform.position + _offset;

        private void Update()
        {
            this.transform.LookAt(_player);
            if (this.transform.position.z >= 69.0f )
            {
                this.transform.position = new Vector3(transform.position.x,transform.position.y , 70.0f);
            }
        }

        public void Zoom(float zooming) => _offset.y -= zooming;
    }
}