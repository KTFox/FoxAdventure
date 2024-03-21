using System;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogues.Editor
{
    public class DialogueEditor : EditorWindow
    {
        // Variables

        private const float _canvasSize = 4000f;
        private const float _backgroundSize = 50f;

        private DialogueSO _selectedDialogue;

        [NonSerialized]
        private GUIStyle _dialogueNodeStyle;

        [NonSerialized]
        private DialogueNodeSO _beingDraggedNode;
        [NonSerialized]
        private Vector2 _draggingOffset;

        [NonSerialized]
        private DialogueNodeSO _creatingNode;
        [NonSerialized]
        private DialogueNodeSO _deletingNode;
        [NonSerialized]
        private DialogueNodeSO _linkingParentNode;

        private Vector2 _scrollViewPosition;
        [NonSerialized]
        private Vector2 _draggingCanvasOffset;


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

                _scrollViewPosition = EditorGUILayout.BeginScrollView(_scrollViewPosition);

                Rect canvas = GUILayoutUtility.GetRect(_canvasSize, _canvasSize);
                Texture2D backgroundTexture = Resources.Load("background") as Texture2D;
                Rect textureCoordinates = new Rect(0, 0, _canvasSize / _backgroundSize, _canvasSize / _backgroundSize);
                GUI.DrawTextureWithTexCoords(canvas, backgroundTexture, textureCoordinates);

                foreach (DialogueNodeSO dialogueNode in _selectedDialogue.DialogueNodes)
                {
                    DrawConnections(dialogueNode);
                }

                foreach (DialogueNodeSO dialogueNode in _selectedDialogue.DialogueNodes)
                {
                    DrawNode(dialogueNode);
                }

                EditorGUILayout.EndScrollView();

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

        private void HandleInteraction()
        {
            if (Event.current.type == EventType.MouseDown && _beingDraggedNode == null)
            {
                _beingDraggedNode = GetNodeAtPoint(Event.current.mousePosition + _scrollViewPosition);

                if (_beingDraggedNode != null)
                {
                    // Click the node

                    _draggingOffset = _beingDraggedNode.Rect.position - Event.current.mousePosition;
                    Selection.activeObject = _beingDraggedNode;
                }
                else
                {
                    // Click the canvas

                    _draggingCanvasOffset = Event.current.mousePosition + _scrollViewPosition;
                    Selection.activeObject = _selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _beingDraggedNode != null)
            {
                // Drag node

                Undo.RecordObject(_selectedDialogue, "Update Node Position");

                _beingDraggedNode.Rect.position = Event.current.mousePosition + _draggingOffset;

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && _beingDraggedNode == null)
            {
                // Drag Canvas

                _scrollViewPosition = _draggingCanvasOffset - Event.current.mousePosition;

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _beingDraggedNode != null)
            {
                _beingDraggedNode = null;
            }
        }

        private void DrawNode(DialogueNodeSO dialogueNode)
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

            DrawLinkButton(dialogueNode);

            if (GUILayout.Button("+"))
            {
                _creatingNode = dialogueNode;
            }

            GUILayout.EndHorizontal();
            GUILayout.EndArea();
        }

        private void DrawConnections(DialogueNodeSO dialogueNode)
        {
            Vector2 startPosition = new Vector2(dialogueNode.Rect.xMax, dialogueNode.Rect.center.y);

            foreach (DialogueNodeSO child in _selectedDialogue.GetAllChildrenOf(dialogueNode))
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

        private DialogueNodeSO GetNodeAtPoint(Vector2 point)
        {
            DialogueNodeSO foundNode = null;

            foreach (DialogueNodeSO dialogueNode in _selectedDialogue.DialogueNodes)
            {
                if (dialogueNode.Rect.Contains(point))
                {
                    foundNode = dialogueNode;
                }
            }

            return foundNode;
        }

        private void DrawLinkButton(DialogueNodeSO dialogueNode)
        {
            if (_linkingParentNode == null)
            {
                if (GUILayout.Button("link"))
                {
                    _linkingParentNode = dialogueNode;
                }
            }
            else if (_linkingParentNode == dialogueNode)
            {
                if (GUILayout.Button("cancel"))
                {
                    _linkingParentNode = null;
                }
            }
            else if (_linkingParentNode.ChildrenUniqueIDs.Contains(dialogueNode.name))
            {
                if (GUILayout.Button("unlink"))
                {
                    Undo.RecordObject(_selectedDialogue, "Remove Dialogue link");
                    _linkingParentNode.ChildrenUniqueIDs.Remove(dialogueNode.name);
                    _linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("child"))
                {
                    Undo.RecordObject(_selectedDialogue, "Add Dialogue link");
                    _linkingParentNode.ChildrenUniqueIDs.Add(dialogueNode.name);
                    _linkingParentNode = null;
                }
            }
        }
    }
}
