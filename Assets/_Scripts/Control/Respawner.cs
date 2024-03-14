using UnityEngine;
using RPG.Attributes;
using System;
using System.Collections;
using RPG.SceneManagement;
using UnityEngine.AI;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField]
        private Transform spawnPosition;
        [SerializeField]
        private float fadeTime = 2f;
        [SerializeField]
        private float fadeDelay = 2f;
        [SerializeField]
        private float healthRegeneratePercentage = 30f;

        private Health playerHealth;

        private void Awake()
        {
            playerHealth = GetComponent<Health>();
        }

        private void Start()
        {
            playerHealth.OnDie.AddListener(Respawn);
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            yield return new WaitForSeconds(fadeDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            GetComponent<NavMeshAgent>().Warp(spawnPosition.position);
            playerHealth.Heal(playerHealth.MaxHealth * healthRegeneratePercentage / 100);
            yield return fader.FadeIn(fadeTime);
        }
    }
}
