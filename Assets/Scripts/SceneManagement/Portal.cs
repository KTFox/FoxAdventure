using RPG.Saving;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement {
    public class Portal : MonoBehaviour {

        enum DestinationIdentifier {
            A, B, C
        }

        [SerializeField]
        [Tooltip("Teleport to the portal that has the same identifier")]
        private DestinationIdentifier identifier;

        [SerializeField]
        private int sceneToLoad;

        [SerializeField]
        private float fadeOutTime;
        [SerializeField]
        private float fadeInTime;
        [SerializeField]
        private float fadeWaitTime;

        [SerializeField]
        private Transform spawnPoint;

        private void OnTriggerEnter(Collider collistion) {
            if (collistion.gameObject.CompareTag("Player")) {
                StartCoroutine(SceneTransition());
            }
        }

        IEnumerator SceneTransition() {
            DontDestroyOnLoad(gameObject);

            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();    

            yield return fader.FadeOut(fadeOutTime);
            savingWrapper.SaveGame();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            savingWrapper.LoadGame();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.SaveGame();

            yield return new WaitForSeconds(fadeWaitTime);
            yield return fader.FadeIn(fadeInTime);

            Destroy(gameObject);
        }

        /// <summary>
        /// Return the portal that has the same identifier with this
        /// </summary>
        /// <returns></returns>
        private Portal GetOtherPortal() {
            Portal[] portals = FindObjectsOfType<Portal>();

            foreach (Portal portal in portals) {
                if (portal == this) continue;
                if (portal.identifier != this.identifier) continue;

                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal portal) {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.position);
            player.transform.rotation = portal.spawnPoint.rotation;
        }
    }
}
