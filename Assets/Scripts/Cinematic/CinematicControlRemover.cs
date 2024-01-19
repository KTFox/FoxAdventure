using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic {
    public class CinematicControlRemover : MonoBehaviour {

        private GameObject player;
        private PlayableDirector playableDirector;

        private void Awake() {
            player = GameObject.FindGameObjectWithTag("Player");
            playableDirector = GetComponent<PlayableDirector>();
        }

        private void OnEnable() {
            playableDirector.played += DisableControl;
            playableDirector.stopped += EnableControl;
        }

        private void OnDisable() {
            playableDirector.played -= DisableControl;
            playableDirector.stopped -= EnableControl;
        }

        private void DisableControl(PlayableDirector director) {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControl(PlayableDirector director) {
            player.GetComponent<PlayerController>().enabled = true;
        }
    }
}
