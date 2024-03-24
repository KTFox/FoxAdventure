using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RPG.Dialogues
{
    public class PlayerConversant : MonoBehaviour
    {
        // Variables

        private AIConversant _currentAIConversant;
        private DialogueSO _currentDialogue;
        private DialogueNodeSO _currentDialogueNode;
        private bool _isChoosing;

        // Events

        public event Action OnConversationUpdated;


        // Methods

        public void StartDialogue(AIConversant newAIConversant, DialogueSO newDialogue)
        {
            _currentAIConversant = newAIConversant;
            _currentDialogue = newDialogue;
            _currentDialogueNode = newDialogue.RootNode;

            TriggerEnterAction();
            OnConversationUpdated?.Invoke();
        }

        public void QuitDialogue()
        {
            _currentDialogue = null;

            TriggerExitAction();

            _currentDialogueNode = null;
            _isChoosing = false;
            _currentAIConversant = null;

            OnConversationUpdated?.Invoke();
        }

        public void SelectChoice(DialogueNodeSO chosenNode)
        {
            _currentDialogueNode = chosenNode;

            TriggerEnterAction();

            _isChoosing = false;
            MoveToNextDialogueNode();
        }

        public void MoveToNextDialogueNode()
        {
            int numberOfPlayerResponses = _currentDialogue.GetPlayerDialogueNodeChildrenOf(_currentDialogueNode).Count();
            if (numberOfPlayerResponses > 0)
            {
                _isChoosing = true;

                TriggerExitAction();
                OnConversationUpdated?.Invoke();

                return;
            }

            DialogueNodeSO[] childNodes = _currentDialogue.GetAIDialogueChildrenOf(_currentDialogueNode).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, childNodes.Count());

            TriggerEnterAction();

            _currentDialogueNode = childNodes[randomIndex];

            TriggerExitAction();
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

        private void TriggerEnterAction()
        {
            if (_currentDialogueNode != null)
            {
                TriggerAction(_currentDialogueNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction()
        {
            if (_currentDialogueNode != null && _currentDialogueNode.GetOnExitAction() != "")
            {
                TriggerAction(_currentDialogueNode.GetOnExitAction());
            }
        }

        private void TriggerAction(string action)
        {
            if (action == "") return;

            foreach (DialogueTrigger trigger in _currentAIConversant.GetComponents<DialogueTrigger>())
            {
                trigger.TriggerAction(action);
            }
        }
    }
}
