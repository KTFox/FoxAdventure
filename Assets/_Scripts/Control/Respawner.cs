using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Cinemachine;
using RPG.SceneManagement;
using RPG.Attributes;

namespace RPG.Control
{
    public class Respawner : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private Transform _reSpawnTrasform;
        [SerializeField]
        private float _fadeTime = 2f;
        [SerializeField]
        private float _fadeDelay = 2f;
        [SerializeField]
        private float _playerHealthRegeneratePercentage = 30f;
        [SerializeField]
        private float _enemyHealthRegeneratePercentage = 30f;

        private Health _playerHealth;


        // Methods

        private void Awake()
        {
            _playerHealth = GetComponent<Health>();
            _playerHealth.OnDeath.AddListener(Health_Death);
        }

        private void Start()
        {
            if (_playerHealth.IsDead)
            {
                Health_Death();
            }
        }

        void Health_Death()
        {
            StartCoroutine(RespawnCoroutine());
        }

        private IEnumerator RespawnCoroutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.SaveData();

            yield return new WaitForSeconds(_fadeDelay);

            Fader fader = FindObjectOfType<Fader>();

            yield return fader.FadeOut(_fadeTime);

            RespawnPlayer();
            ResetEnemies();
            savingWrapper.SaveData();

            yield return fader.FadeIn(_fadeTime);
        }

        private void RespawnPlayer()
        {
            GetComponent<NavMeshAgent>().Warp(_reSpawnTrasform.position);
            _playerHealth.Heal(_playerHealth.MaxHealth * _playerHealthRegeneratePercentage / 100);

            // Reset player follow camera
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            Vector3 positionDelta = _reSpawnTrasform.position - transform.position;

            if (activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }

        private void ResetEnemies()
        {
            foreach (var enemyController in FindObjectsOfType<AIController>())
            {
                var enemyHealth = enemyController.GetComponent<Health>();

                if (enemyHealth && !enemyHealth.IsDead)
                {
                    enemyController.Reset();
                    enemyHealth.Heal(enemyHealth.MaxHealth * _enemyHealthRegeneratePercentage / 100);
                }
            }
        }
    }
}
