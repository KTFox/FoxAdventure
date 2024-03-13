using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using RPG.Control;
using RPG.Core;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        [Tooltip("Teleport to the portal that has the same identifier")]
        [SerializeField]
        private DestinationIdentifier identifier;

        enum DestinationIdentifier
        {
            A, B, C
        }

        [SerializeField]
        private int sceneToLoad;
        [SerializeField]
        private float fadeOutTime;
        [SerializeField]
        private float fadeInTime;
        [SerializeField]
        private float fadeInWaitTime;
        [SerializeField]
        private Transform spawnPoint;

        private void OnTriggerEnter(Collider collistion)
        {
            if (collistion.gameObject.CompareTag("Player"))
            {
                StartCoroutine(SceneTransition());
            }
        }

        IEnumerator SceneTransition()
        {
            DontDestroyOnLoad(gameObject);

            //fader and savingWrapper are persistent objects
            Fader fader = FindObjectOfType<Fader>();
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            DisableControll();

            yield return fader.FadeOut(fadeOutTime);
            savingWrapper.Save();

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            DisableControll();
            savingWrapper.Load();

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            savingWrapper.Save();

            yield return new WaitForSeconds(fadeInWaitTime);

            fader.FadeIn(fadeInTime);
            EnableControll();

            Destroy(gameObject);
        }

        private void DisableControll()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnableControll()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<PlayerController>().enabled = true;
        }

        /// <summary>
        /// Return the portal that has the same identifier with this
        /// </summary>
        /// <returns></returns>
        private Portal GetOtherPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();

            foreach (Portal portal in portals)
            {
                if (portal == this) continue;
                if (portal.identifier != this.identifier) continue;

                return portal;
            }

            return null;
        }

        private void UpdatePlayer(Portal portal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(portal.spawnPoint.position);
            player.transform.rotation = portal.spawnPoint.rotation;
        }
    }
}
