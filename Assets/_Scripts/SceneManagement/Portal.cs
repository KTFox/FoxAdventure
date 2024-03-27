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
        enum DestinationIdentifier
        {
            A, B, C
        }

        // Variables

        [Tooltip("Teleport to the portal that has the same _portalIdentifier")]
        [SerializeField]
        private DestinationIdentifier _portalIdentifier;
        [SerializeField]
        private int _sceneIndexToLoad;
        [SerializeField]
        private float _fadeTime;
        [SerializeField]
        private float _waitingTimeBeforeFadingIn;
        [SerializeField]
        private Transform _positionToSpawn;


        // Methods

        private void OnTriggerEnter(Collider collistion)
        {
            if (collistion.gameObject.CompareTag("Player"))
            {
                StartCoroutine(SceneTransitionCoroutine());
            }
        }

        private IEnumerator SceneTransitionCoroutine()
        {
            DontDestroyOnLoad(gameObject);

            var fader = FindObjectOfType<Fader>();
            var savingWrapper = FindObjectOfType<SavingWrapper>();

            DisablePlayerController();

            yield return fader.FadeOut(_fadeTime);

            savingWrapper.SaveGameState();

            yield return SceneManager.LoadSceneAsync(_sceneIndexToLoad);

            DisablePlayerController();
            savingWrapper.RestoreGameState();

            Portal otherPortal = GetSameIdentifierPortal();
            UpdatePlayerTransform(otherPortal);

            savingWrapper.SaveGameState();

            yield return new WaitForSeconds(_waitingTimeBeforeFadingIn);
            yield return fader.FadeIn(_fadeTime);

            EnablePlayerController();
            Destroy(gameObject);
        }

        private void DisablePlayerController()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().enabled = false;
        }

        private void EnablePlayerController()
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<PlayerController>().enabled = true;
        }

        private void UpdatePlayerTransform(Portal portal)
        {
            var player = GameObject.FindGameObjectWithTag("Player");

            player.GetComponent<NavMeshAgent>().Warp(portal._positionToSpawn.position);
            player.transform.rotation = portal._positionToSpawn.rotation;
        }

        private Portal GetSameIdentifierPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();

            foreach (Portal portal in portals)
            {
                if (portal == this) continue;
                if (portal._portalIdentifier != _portalIdentifier) continue;

                return portal;
            }

            return null;
        }
    }
}
