using UnityEngine;
using RPG.Control;

namespace RPG.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        private PlayerController playerController;

        private void Awake()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            Time.timeScale = 0f;
            playerController.enabled = false;
        }

        private void OnDisable()
        {
            Time.timeScale = 1f;
            playerController.enabled = true;
        }
    }
}
