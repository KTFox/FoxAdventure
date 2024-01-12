using RPG.Control;
using RPG.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace RPG.Cinematic {
    public class CinematicControlRemover : MonoBehaviour {

        private GameObject player;

        private void Start() {
            player = GameObject.FindGameObjectWithTag("Player");

            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
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
