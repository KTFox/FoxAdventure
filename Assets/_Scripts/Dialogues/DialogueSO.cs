using System;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Dialogues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        // Variables

        [SerializeField]
        private List<DialogueNode> _dialogueNodes = new List<DialogueNode>();

        private Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

        // Properties

        public IEnumerable<DialogueNode> DialogueNodes => _dialogueNodes;
        public DialogueNode RootNode => _dialogueNodes[0];


        // Methods

#if UNITY_EDITOR
        private void Awake()
        {
            if (_dialogueNodes.Count == 0)
            {
                DialogueNode rootNode = new DialogueNode();
                rootNode.UniqueID = Guid.NewGuid().ToString();

                _dialogueNodes.Add(rootNode);
            }
        }

        private void OnValidate()
        {
            _nodeLookup.Clear();

            foreach (DialogueNode node in _dialogueNodes)
            {
                _nodeLookup[node.UniqueID] = node;
            }
        }
#endif

        public IEnumerable<DialogueNode> GetAllChildrenOf(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.ChildrenUniqueIDs)
            {
                if (_nodeLookup.ContainsKey(childID))
                {
                    yield return _nodeLookup[childID];
                }
            }
        }

        public void CreateNewNode(DialogueNode parentNode)
        {
            var newNode = new DialogueNode
            {
                UniqueID = Guid.NewGuid().ToString()
            };

            parentNode.ChildrenUniqueIDs.Add(newNode.UniqueID);
            _dialogueNodes.Add(newNode);
            OnValidate();
        }

        public void DeleteNode(DialogueNode nodeToDelete)
        {
            _dialogueNodes.Remove(nodeToDelete);

            foreach (DialogueNode node in _dialogueNodes)
            {
                node.ChildrenUniqueIDs.Remove(nodeToDelete.UniqueID);
            }

            OnValidate();
        }
    }
}
