using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Cinemachine;
using RPG.SceneManagement;
using RPG.Attributes;
using System;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField]
        private Transform reSpawnPosition;
        [SerializeField]
        private float fadeTime = 2f;
        [SerializeField]
        private float fadeDelay = 2f;
        [SerializeField]
        private float playerHealthRegeneratePercentage = 30f;
        [SerializeField]
        private float enemyHealthRegeneratePercentage = 30f;

        private Health playerHealth;

        private void Awake()
        {
            playerHealth = GetComponent<Health>();
            playerHealth.OnDie.AddListener(Respawn);
        }

        private void Start()
        {
            if (playerHealth.IsDead)
            {
                Respawn();
            }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.SaveData();
            yield return new WaitForSeconds(fadeDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            RespawnPlayer();
            ResetEnemies();
            savingWrapper.SaveData();
            yield return fader.FadeIn(fadeTime);
        }

        private void RespawnPlayer()
        {
            Vector3 positionDelta = reSpawnPosition.position - transform.position;
            GetComponent<NavMeshAgent>().Warp(reSpawnPosition.position);
            playerHealth.Heal(playerHealth.MaxHealth * playerHealthRegeneratePercentage / 100);
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if (activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }

        private void ResetEnemies()
        {
            foreach (AIController enemyController in FindObjectsOfType<AIController>())
            {
                Health enemyHealth = enemyController.GetComponent<Health>();
                if (enemyHealth && !enemyHealth.IsDead)
                {
                    enemyController.ResetEnemy();
                    enemyHealth.Heal(enemyHealth.MaxHealth * enemyHealthRegeneratePercentage / 100);
                }
            }
        }
    }
}
