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
                ProcessEvents();

                foreach (DialogueNode dialogueNode in selectedDialogue.DialogueNodes)
                {
                    OnGUINode(dialogueNode);
                }
            }
            else
            {
                EditorGUILayout.LabelField("No dialogue selected.");
            }
        }

        void ProcessEvents()
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

        void OnGUINode(DialogueNode dialogueNode)
        {
            GUILayout.BeginArea(dialogueNode.Rect, dialogueNodeStyle);
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.LabelField("Nodes: ");
            string newText = EditorGUILayout.TextField(dialogueNode.Text);
            string newUniqueID = EditorGUILayout.TextField(dialogueNode.UniqueId);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(selectedDialogue, "Update DialogueSO");

                dialogueNode.Text = newText;
                dialogueNode.UniqueId = newUniqueID;
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
