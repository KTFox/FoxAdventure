using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using RPG.Core;

namespace RPG.Dialogues
{
    public class PlayerConversant : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private string _playerName;

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
            int numberOfPlayerResponses = GetFilteredNodeOnCondition(_currentDialogue.GetPlayerDialogueNodeChildrenOf(_currentDialogueNode)).Count();
            if (numberOfPlayerResponses > 0)
            {
                _isChoosing = true;

                TriggerExitAction();
                OnConversationUpdated?.Invoke();

                return;
            }

            DialogueNodeSO[] childNodes = GetFilteredNodeOnCondition(_currentDialogue.GetAIDialogueChildrenOf(_currentDialogueNode)).ToArray();
            int randomIndex = UnityEngine.Random.Range(0, childNodes.Count());

            TriggerEnterAction();

            _currentDialogueNode = childNodes[randomIndex];

            TriggerExitAction();
            OnConversationUpdated?.Invoke();
        }

        public string GetCurrentConversantName()
        {
            if (_isChoosing)
            {
                return _playerName;
            }
            else
            {
                return _currentAIConversant.ConversantName;
            }
        }

        public IEnumerable<DialogueNodeSO> GetChoices()
        {
            return GetFilteredNodeOnCondition(_currentDialogue.GetPlayerDialogueNodeChildrenOf(_currentDialogueNode));
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
            return GetFilteredNodeOnCondition(_currentDialogue.GetDialogueNodeChildrenOf(_currentDialogueNode)).Count() > 0;
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

        private IEnumerable<DialogueNodeSO> GetFilteredNodeOnCondition(IEnumerable<DialogueNodeSO> inputNodes)
        {
            foreach (DialogueNodeSO node in inputNodes)
            {
                if (node.CheckCondition(GetEvaluators()))
                {
                    yield return node;
                }
            }
        }

        private IEnumerable<IPredicateEvaluator> GetEvaluators()
        {
            return GetComponents<IPredicateEvaluator>();
        }
    }
}
