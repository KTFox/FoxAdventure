using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent Hit;

        public void OnHit()
        {
            Hit?.Invoke();
        }
    }
}