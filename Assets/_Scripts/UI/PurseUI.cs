using TMPro;
using UnityEngine;
using RPG.Inventories;

namespace RPG.UI
{
    public class PurseUI : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI balanceAmount;

        private Purse playerPurse;

        private void Awake()
        {
            playerPurse = GameObject.FindGameObjectWithTag("Player").GetComponent<Purse>();
        }

        private void Start()
        {
            RefreshUI();

            playerPurse.OnPurseUpdated += RefreshUI;
        }

        void RefreshUI()
        {
            balanceAmount.text = $"${playerPurse.CurrentBalance:N2}";
        }
    }
}
