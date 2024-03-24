using System.Collections.Generic;
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
        private bool _isChoosing;


        // Methods

        private void Awake()
        {
            _currentDialogueNode = _currentDialogue.RootNode;
        }

        public void SelectChoice(DialogueNodeSO chosenNode)
        {
            _currentDialogueNode = chosenNode;
            _isChoosing = false;
            MoveToNextDialogueNode();
        }

        public IEnumerable<DialogueNodeSO> GetChoices()
        {
            foreach (DialogueNodeSO node in _currentDialogue.GetPlayerDialogueNodeChildrenOf(_currentDialogueNode))
            {
                yield return node;
            }
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
            int numberOfPlayerResponses = _currentDialogue.GetPlayerDialogueNodeChildrenOf(_currentDialogueNode).Count();
            if (numberOfPlayerResponses > 0)
            {
                _isChoosing = true;
                return;
            }

            DialogueNodeSO[] childNodes = _currentDialogue.GetAIDialogueChildrenOf(_currentDialogueNode).ToArray();
            int randomIndex = Random.Range(0, childNodes.Count());

            _currentDialogueNode = childNodes[randomIndex];
        }

        public bool IsChoosing()
        {
            return _isChoosing;
        }

        public bool HasNextDialogueNode()
        {
            return _currentDialogue.GetDialogueNodeChildrenOf(_currentDialogueNode).Count() > 0;
        }
    }
}
