using RPG.Dialogues;
using System.Linq;
using TMPro;
using UnityEngine;

namespace RPG
{
    public class DebugPanel : MonoBehaviour
    {
        [SerializeField]
        private GameObject _button;
        [SerializeField]
        private TextMeshProUGUI _text;

        private PlayerConversant _playerConversant;


        private void Start()
        {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
        }

        private void Update()
        {
            _text.text = $"Next button is active: {_button.activeInHierarchy}" +
                $"\nHas Next DialogueNode: {_playerConversant.HasNextDialogueNode()}";

            if (_playerConversant.CurrentDialogue == null) return;
            for (int i = 0; i < _playerConversant.CurrentDialogue.DialogueNodes.ToArray().Length; i++)
            {
                _text.text += $"\n{_playerConversant.CurrentDialogue.Nodes[i].GetText()}";
            }
        }
    }
}
