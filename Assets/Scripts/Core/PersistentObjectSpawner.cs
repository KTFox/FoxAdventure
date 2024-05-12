using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        // Variables

        [SerializeField]
        private GameObject _persistentObjectPrefab;

        private static bool HAS_SPAWNED;


        // Methods

        private void Awake()
        {
            if (HAS_SPAWNED) return;

            SpawnPersistentObject();
            HAS_SPAWNED = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObejct = Instantiate(_persistentObjectPrefab);
            DontDestroyOnLoad(persistentObejct);
        }
    }
}
