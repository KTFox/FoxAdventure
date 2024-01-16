using RPG.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Saving {
    [ExecuteAlways]
    public class SaveableEntity : MonoBehaviour {

        [SerializeField]
        private string uniqueIdentifier = "";

        public object CaptureState() {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) {
            transform.position = ((SerializableVector3)state).ToVector();
            GetComponent<NavMeshAgent>().enabled = false;
            GetComponent<NavMeshAgent>().enabled = true;
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public string GetUniqueIdentifier() {
            return uniqueIdentifier;
        }

        /// <summary>
        /// Auto generate unique identifier in Edit Mode.
        /// This Update will be not included in Build Project
        /// </summary>
        private void Update() {
            if (Application.IsPlaying(gameObject)) return;
            if (string.IsNullOrEmpty(gameObject.scene.path)) return; //path will be null or empty when gameObject is in prefab folder 

            //In order to change the values that are being stored into the scene file or prefab, use SerializedObject and SerializedProperty 
            SerializedObject serializedObject = new SerializedObject(this);
            SerializedProperty property = serializedObject.FindProperty("uniqueIdentifier");

            //Generate unique identifier 
            if (string.IsNullOrEmpty(property.stringValue)) {
                property.stringValue = System.Guid.NewGuid().ToString();
                serializedObject.ApplyModifiedProperties();
            }

            Debug.Log("Editting");
        }
    }
}
