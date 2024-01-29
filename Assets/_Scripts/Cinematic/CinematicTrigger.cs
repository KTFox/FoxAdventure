using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic {
    public class CinematicTrigger : MonoBehaviour {

        private bool alreadyTriggered;

        private void OnTriggerEnter(Collider collision) {
            if (collision.CompareTag("Player") && !alreadyTriggered) {
                GetComponent<PlayableDirector>().Play();
                alreadyTriggered = true;
            }
        }
    }
}
