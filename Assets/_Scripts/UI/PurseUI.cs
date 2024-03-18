using TMPro;
using UnityEngine;
using RPG.Inventories;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _balanceText;
        private Purse _playerPurse;


        // Methods

        private void Awake()
        {
            _playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();
        }

        private void Start()
        {
            _playerPurse_OnPurseUpdated();

            _playerPurse.OnPurseUpdated += _playerPurse_OnPurseUpdated;
        }

        void _playerPurse_OnPurseUpdated()
        {
            _balanceText.text = $"${_playerPurse.CurrentBalance:N2}";
        }
    }
}
