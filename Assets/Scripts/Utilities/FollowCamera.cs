using UnityEngine;

namespace Spark.Utilities
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField] Transform _player;
        [SerializeField] Vector3 _offset = new Vector3(0.0f, 15.0f, -10.0f);

        void LateUpdate() => transform.position = _player.transform.position + _offset;
    }
}