using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace RPG.Dialogues.Editor
{
    public class DialogueEditor : EditorWindow
    {
        // Variables

        private DialogueSO selectedDialogue;
        private GUIStyle dialogueNodeStyle;

        private DialogueNode beingDraggedNode;
        private Vector2 draggingOffset;


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

            dialogueNodeStyle = new GUIStyle();
            dialogueNodeStyle.normal.background = EditorGUIUtility.Load("Node0") as Texture2D;
            dialogueNodeStyle.padding = new RectOffset(10, 10, 10, 10);
            dialogueNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        void OnSelectionChanged()
        {
            DialogueSO newDialogue = Selection.activeObject as DialogueSO;

            if (newDialogue != null)
            {
                selectedDialogue = newDialogue;
                Repaint();
            }
        }

        private void OnGUI()
        {
            if (selectedDialogue != null)
            {
                HandleInteractionWithGUI();

                foreach (DialogueNode dialogueNode in selectedDialogue.DialogueNodes)
                {
                    CreateNodeGUI(dialogueNode);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No dialogue selected.");
            }
        }

        void HandleInteractionWithGUI()
        {
            if (Event.current.type == EventType.MouseDown && beingDraggedNode == null)
            {
                beingDraggedNode = GetNodeAtPoint(Event.current.mousePosition);

                if (beingDraggedNode != null)
                {
                    draggingOffset = beingDraggedNode.Rect.position - Event.current.mousePosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && beingDraggedNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Update Node Position");
                beingDraggedNode.Rect.position = Event.current.mousePosition + draggingOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && beingDraggedNode != null)
            {
                beingDraggedNode = null;
            }
        }

        void CreateNodeGUI(DialogueNode dialogueNode)
        {
            GUILayout.BeginArea(dialogueNode.Rect, dialogueNodeStyle);
            EditorGUI.BeginChangeCheck();

            string newUniqueID = EditorGUILayout.TextField(dialogueNode.UniqueID);
            string newText = EditorGUILayout.TextField(dialogueNode.Text);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update DialogueSO");

                dialogueNode.Text = newText;
                dialogueNode.UniqueID = newUniqueID;
            }

            foreach (DialogueNode childNode in selectedDialogue.GetAllChildrenNodes(dialogueNode))
            {
                EditorGUILayout.LabelField(childNode.Text);
            }

            GUILayout.EndArea();
        }

        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;

            foreach (DialogueNode dialogueNode in selectedDialogue.DialogueNodes)
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
