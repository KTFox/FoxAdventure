using UnityEngine;
using UnityEngine.Events;

namespace RPG.Combat {
    public class Weapon : MonoBehaviour {

        [SerializeField]
        private UnityEvent OnHit;

        public void CallHitEvent() {
            OnHit?.Invoke();
        }
    }
}
