using System.Linq;
using UnityEngine;

namespace RPG.Dialogues
{
    public class PlayerConversant : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private DialogueSO _currentDialogue;

        private DialogueNodeSO _currentDialogueNode;


        // Methods

        private void Awake()
        {
            _currentDialogueNode = _currentDialogue.RootNode;
        }

        public string GetCurrentDialogueText()
        {
            if (_currentDialogue == null)
            {
                return "";
            }

            return _currentDialogueNode.GetText();
        }

        public void MoveToNextDialogueNode()
        {
            DialogueNodeSO[] childNodes = _currentDialogue.GetAllChildrenOf(_currentDialogueNode).ToArray();
            int randomIndex = Random.Range(0, childNodes.Count());

            _currentDialogueNode = childNodes[randomIndex];
        }

        public bool HasNextDialogueNode()
        {
            return _currentDialogue.GetAllChildrenOf(_currentDialogueNode).Count() > 0;
        }
    }
}
