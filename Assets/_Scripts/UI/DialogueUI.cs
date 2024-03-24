using TMPro;
using UnityEngine;
using RPG.Dialogues;

namespace RPG.UI
{
    public class DialogueUI : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private TextMeshProUGUI _AIText;

        private PlayerConversant _playerConversant;


        // Methods

        private void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();

            _AIText.text = _playerConversant.GetText();
        }
    }
}
