using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField]
        private GameObject persistentObjectPrefab;

        private static bool hasSpawned;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObject();
            hasSpawned = true;
        }

        private void SpawnPersistentObject()
        {
            GameObject persistentObejct = Instantiate(persistentObjectPrefab);
            DontDestroyOnLoad(persistentObejct);
        }
    }
}
