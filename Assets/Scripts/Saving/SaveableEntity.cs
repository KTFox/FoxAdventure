using UnityEngine;

namespace RPG.Saving {
    public class SaveableEntity : MonoBehaviour {

        public string GetUniqueIdentifier() {
            return "Character";
        }

        public object CaptureState() {
            Debug.Log($"Capture state for {GetUniqueIdentifier()}");
            return null;
        }

        public void RestoreState(object state) {
            Debug.Log($"Restore state for {GetUniqueIdentifier()}");
        }
    }
}
