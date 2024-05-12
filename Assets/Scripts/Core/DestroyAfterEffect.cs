using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        [SerializeField]
        private GameObject _targetToDestroy;

        private void Update()
        {
            if (!GetComponent<ParticleSystem>().IsAlive())
            {
                if (_targetToDestroy != null)
                {
                    Destroy(_targetToDestroy);
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }
    }
}