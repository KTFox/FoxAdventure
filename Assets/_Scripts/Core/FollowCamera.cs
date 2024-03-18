using UnityEngine;

namespace RPG.Core
{
    public class FollowCamera : MonoBehaviour
    {
        [SerializeField]
        private Transform _targetToFollow;

        private void LateUpdate()
        {
            transform.position = _targetToFollow.position;
        }
    }
}

