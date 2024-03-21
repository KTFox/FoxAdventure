using System;
using System.Collections.Generic;
using System.Xml;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Dialogue")]
    public class DialogueSO : ScriptableObject
    {
        // Variables

        [SerializeField]
        private List<DialogueNodeSO> _dialogueNodes = new List<DialogueNodeSO>();

        private Dictionary<string, DialogueNodeSO> _nodeLookup = new Dictionary<string, DialogueNodeSO>();

        // Properties

        public IEnumerable<DialogueNodeSO> DialogueNodes => _dialogueNodes;
        public DialogueNodeSO RootNode => _dialogueNodes[0];


        // Methods

#if UNITY_EDITOR
        private void Awake()
        {
            if (_dialogueNodes.Count == 0)
            {
                CreateNewNode(null);
            }
        }

        private void OnValidate()
        {
            _nodeLookup.Clear();

            foreach (DialogueNodeSO node in _dialogueNodes)
            {
                _nodeLookup[node.name] = node;
            }
        }
#endif

        public IEnumerable<DialogueNodeSO> GetAllChildrenOf(DialogueNodeSO parentNode)
        {
            foreach (string childID in parentNode.ChildrenUniqueIDs)
            {
                if (_nodeLookup.ContainsKey(childID))
                {
                    yield return _nodeLookup[childID];
                }
            }
        }

        public void CreateNewNode(DialogueNodeSO parentNode)
        {
            var newNode = CreateInstance<DialogueNodeSO>();
            newNode.name = Guid.NewGuid().ToString();
            Undo.RegisterCreatedObjectUndo(newNode, "Added new node");

            _dialogueNodes.Add(newNode);

            if (parentNode != null)
            {
                parentNode.ChildrenUniqueIDs.Add(newNode.name);
            }

            OnValidate();
        }

        public void DeleteNode(DialogueNodeSO nodeToDelete)
        {
            _dialogueNodes.Remove(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);

            foreach (DialogueNodeSO node in _dialogueNodes)
            {
                node.ChildrenUniqueIDs.Remove(nodeToDelete.name);
            }

            OnValidate();
        }
    }
}
