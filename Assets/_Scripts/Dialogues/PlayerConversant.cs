using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogues
{
    public class PlayerConversant : MonoBehaviour
    {
        // Variables

        private DialogueSO _currentDialogue;
        private DialogueNodeSO _currentDialogueNode;
        private bool _isChoosing;

        // Events

        public event Action OnConversationUpdated;


        // Methods

        public void StartDialogue(DialogueSO dialogueToStart)
        {
            _currentDialogue = dialogueToStart;
            _currentDialogueNode = dialogueToStart.RootNode;

            OnConversationUpdated?.Invoke();
        }

        public void QuitDialogue()
        {
            _currentDialogue = null;
            _currentDialogueNode = null;
            _isChoosing = false;

            OnConversationUpdated?.Invoke();
        }

        public void SelectChoice(DialogueNodeSO chosenNode)
        {
            _currentDialogueNode = chosenNode;
            _isChoosing = false;
            MoveToNextDialogueNode();
        }

        public void MoveToNextDialogueNode()
        {
            int numberOfPlayerResponses = _currentDialogue.GetPlayerDialogueNodeChildrenOf(_currentDialogueNode).Count();
            if (numberOfPlayerResponses > 0)
            {
                _isChoosing = true;

                OnConversationUpdated?.Invoke();

                return;
            }

            DialogueNodeSO[] childNodes = _currentDialogue.GetAIDialogueChildrenOf(_currentDialogueNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, childNodes.Count());

            _currentDialogueNode = childNodes[randomIndex];

            OnConversationUpdated?.Invoke();
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

        public bool IsChoosing()
        {
            return _isChoosing;
        }

        public bool HasNextDialogueNode()
        {
            return _currentDialogue.GetDialogueNodeChildrenOf(_currentDialogueNode).Count() > 0;
        }

        public bool IsActiveDialogue()
        {
            return _currentDialogue != null;
        }
    }
}
