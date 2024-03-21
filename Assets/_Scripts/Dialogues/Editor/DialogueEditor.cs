using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogues.Editor
{
    public class DialogueEditor : EditorWindow
    {
        // Variables

        private DialogueSO _selectedDialogue;

        [NonSerialized]
        private GUIStyle _dialogueNodeStyle;

        [NonSerialized]
        private DialogueNode _beingDraggedNode;
        [NonSerialized]
        private Vector2 _draggingOffset;

        [NonSerialized]
        private DialogueNode _creatingNode;
        [NonSerialized]
        private DialogueNode _deletingNode;


        // Methods

        #region Editor Window
        [MenuItem("Window/Dialogue Editor")]
        public static void ShowDialogueEditorWindow()
        {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceId, int line)
        {
            DialogueSO dialogueSO = EditorUtility.InstanceIDToObject(instanceId) as DialogueSO;

            if (dialogueSO != null)
            {
                ShowDialogueEditorWindow();

                return true;
            }

            return false;
        }
        #endregion

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            _dialogueNodeStyle = new GUIStyle();
            _dialogueNodeStyle.normal.background = EditorGUIUtility.Load("Node0") as Texture2D;
            _dialogueNodeStyle.padding = new RectOffset(10, 10, 10, 10);
            _dialogueNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        void OnSelectionChanged()
        {
            DialogueSO newDialogue = Selection.activeObject as DialogueSO;

            if (newDialogue != null)
            {
                _selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (_selectedDialogue != null)
            {
                HandleInteraction();

                foreach (DialogueNode dialogueNode in _selectedDialogue.DialogueNodes)
                {
                    DrawConnections(dialogueNode);
                }

                foreach (DialogueNode dialogueNode in _selectedDialogue.DialogueNodes)
                {
                    DrawNode(dialogueNode);
                }

                if (_creatingNode != null)
                {
                    Undo.RecordObject(_selectedDialogue, "Added Dialogue Node");
                    _selectedDialogue.CreateNewNode(_creatingNode);
                    _creatingNode = null;
                }

                if (_deletingNode != null)
                {
                    Undo.RecordObject(_selectedDialogue, "Deleted Dialogue Node");
                    _selectedDialogue.DeleteNode(_deletingNode);
                    _deletingNode = null;
                }
            }
            else
            {
                EditorGUILayout.LabelField("No dialogue selected.");
            }
        }

        void HandleInteraction()
        {
            if (Event.current.type == EventType.MouseDown && _beingDraggedNode == null)
            {
                _beingDraggedNode = GetNodeAtPoint(Event.current.mousePosition);

                if (_beingDraggedNode != null)
                {
                    _draggingOffset = _beingDraggedNode.Rect.position - Event.current.mousePosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _beingDraggedNode != null)
            {
                Undo.RecordObject(_selectedDialogue, "Update Node Position");
                _beingDraggedNode.Rect.position = Event.current.mousePosition + _draggingOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _beingDraggedNode != null)
            {
                _beingDraggedNode = null;
            }
        }

        void DrawNode(DialogueNode dialogueNode)
        {
            GUILayout.BeginArea(dialogueNode.Rect, _dialogueNodeStyle);
            EditorGUI.BeginChangeCheck();

            string newText = EditorGUILayout.TextField(dialogueNode.Text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(_selectedDialogue, "Update DialogueSO");

                dialogueNode.Text = newText;
            }

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("-"))
            {
                _deletingNode = dialogueNode;
            }

            if (GUILayout.Button("+"))
            {
                _creatingNode = dialogueNode;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        void DrawConnections(DialogueNode dialogueNode)
        {
            Vector2 startPosition = new Vector2(dialogueNode.Rect.xMax, dialogueNode.Rect.center.y);

            foreach (DialogueNode child in _selectedDialogue.GetAllChildrenOf(dialogueNode))
            {
                Vector2 endPosition = new Vector2(child.Rect.xMin, child.Rect.center.y);
                Vector2 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;

                Handles.DrawBezier(startPosition, endPosition,
                    startPosition + controlPointOffset,
                    endPosition - controlPointOffset,
                    Color.white, null, 4f);
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach (DialogueNode dialogueNode in _selectedDialogue.DialogueNodes)
            {
                if (dialogueNode.Rect.Contains(point))
                {
                    foundNode = dialogueNode;
                }
            }

            return foundNode;
        }
    }
}
