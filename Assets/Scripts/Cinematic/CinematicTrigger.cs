using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic {
    public class CinematicTrigger : MonoBehaviour {

        private PlayableDirector playableDirector;
        private bool alreadyTriggered;

        private void Start() {
            playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnTriggerEnter(Collider collision) {
            if (collision.CompareTag("Player") && !alreadyTriggered) {
                playableDirector.Play();
                alreadyTriggered = true;
            }
        }
    }
}
