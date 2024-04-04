using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogues
{
    [CreateAssetMenu(menuName = "ScriptableObject/Dialogue")]
    public class DialogueSO : ScriptableObject, ISerializationCallbackReceiver
    {
        // Variables

        [SerializeField]
        private List<DialogueNodeSO> _dialogueNodes = new List<DialogueNodeSO>();
        [SerializeField]
        private Vector2 _newNodeOffset = new Vector2(250, 0);

        private Dictionary<string, DialogueNodeSO> _nodeLookup = new Dictionary<string, DialogueNodeSO>();

        // Properties

        public IEnumerable<DialogueNodeSO> DialogueNodes => _dialogueNodes;
        public DialogueNodeSO RootNode => _dialogueNodes[0];


        // Methods

        private void OnValidate()
        {
            if (_dialogueNodes[0] == null) return;

            _nodeLookup.Clear();

            foreach (DialogueNodeSO node in _dialogueNodes)
            {
                _nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNodeSO> GetPlayerDialogueNodeChildrenOf(DialogueNodeSO parentNode)
        {
            foreach (DialogueNodeSO node in GetDialogueNodeChildrenOf(parentNode))
            {
                if (node.IsPlayerDialogue())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNodeSO> GetAIDialogueChildrenOf(DialogueNodeSO parentNode)
        {
            foreach (DialogueNodeSO node in GetDialogueNodeChildrenOf(parentNode))
            {
                if (!node.IsPlayerDialogue())
                {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNodeSO> GetDialogueNodeChildrenOf(DialogueNodeSO parentNode)
        {
            foreach (string childID in parentNode.GetChildrenUniqueIds())
            {
                if (_nodeLookup.ContainsKey(childID))
                {
                    yield return _nodeLookup[childID];
                }
            }
        }

#if UNITY_EDITOR
        public void CreateNewNode(DialogueNodeSO parentNode)
        {
            DialogueNodeSO newNode = InstantiateNode(parentNode);

            Undo.RegisterCreatedObjectUndo(newNode, "Added new node");
            Undo.RecordObject(this, "Added Dialogue Node");

            AddNode(newNode);
        }

        public void DeleteNode(DialogueNodeSO nodeToDelete)
        {
            Undo.RecordObject(this, "Deleted Dialogue Node");
            _dialogueNodes.Remove(nodeToDelete);

            OnValidate();

            foreach (DialogueNodeSO node in _dialogueNodes)
            {
                node.RemoveChild(nodeToDelete.name);
            }

            Undo.DestroyObjectImmediate(nodeToDelete);
        }

        private DialogueNodeSO InstantiateNode(DialogueNodeSO parentNode)
        {
            var newNode = CreateInstance<DialogueNodeSO>();
            newNode.name = Guid.NewGuid().ToString();

            if (parentNode != null)
            {
                parentNode.AddChild(newNode.name);
                newNode.SetIsPlayerDialogue(!parentNode.IsPlayerDialogue());
                newNode.SetPosition(parentNode.GetPosition() + _newNodeOffset);
            }

            return newNode;
        }

        private void AddNode(DialogueNodeSO newNode)
        {
            _dialogueNodes.Add(newNode);
            OnValidate();
        }
#endif

        #region ISerializationCallbackReceiver implements
        public void OnBeforeSerialize()
        {
#if UNITY_EDITOR
            if (AssetDatabase.GetAssetPath(this) != "")
            {
                foreach (DialogueNodeSO node in _dialogueNodes)
                {
                    if (AssetDatabase.GetAssetPath(node) == "")
                        AssetDatabase.AddObjectToAsset(node, this);
                }

                if (_dialogueNodes.Count == 0)
                {
                    DialogueNodeSO newNode = InstantiateNode(null);
                    AddNode(newNode);
                }

            }
#endif
        }

        public void OnAfterDeserialize()
        {

        }
        #endregion
    }
}
