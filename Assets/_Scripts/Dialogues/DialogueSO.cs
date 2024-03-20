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
        private List<DialogueNode> _dialogueNodes;

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
                _dialogueNodes.Add(new DialogueNode());
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

        public IEnumerable<DialogueNode> GetAllChildrenNodes(DialogueNode parentNode)
        {
            foreach (string childID in parentNode.ChildrenUniqueIDs)
            {
                if (_nodeLookup.ContainsKey(childID))
                {
                    yield return _nodeLookup[childID];
                }
            }
        }
    }
}
