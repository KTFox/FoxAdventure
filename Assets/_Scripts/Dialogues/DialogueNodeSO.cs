using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace RPG.Dialogues
{
    public class DialogueNodeSO : ScriptableObject
    {
        // Variables

        [SerializeField]
        private string _text;
        [SerializeField]
        private List<string> _childrenUniqueIds = new List<string>();
        [SerializeField]
        private Rect _rect = new Rect(0, 0, 200, 100);


        // Methods

        public string GetText()
        {
            return _text;
        }

        public List<string> GetChildrenUniqueIds()
        {
            return _childrenUniqueIds;
        }

        public Rect GetRect()
        {
            return _rect;
        }

        public Vector2 GetPosition()
        {
            return _rect.position;
        }

#if UNITY_EDITOR
        public void SetText(string text)
        {
            if (text != _text)
            {
                Undo.RecordObject(this, "Update node text");
                _text = text;
                EditorUtility.SetDirty(this);
            }
        }

        public void SetPosition(Vector3 newPosition)
        {
            Undo.RecordObject(this, "Update node newPosition");
            _rect.position = newPosition;
            EditorUtility.SetDirty(this);
        }

        public void AddChild(string childId)
        {
            Undo.RecordObject(this, "Add child");
            _childrenUniqueIds.Add(childId);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChild(string childId)
        {
            Undo.RecordObject(this, "Remove child");
            _childrenUniqueIds.Remove(childId);
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
