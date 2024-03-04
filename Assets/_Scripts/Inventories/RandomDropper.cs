using RPG.Stats;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        [Tooltip("How far the pickups can be scattered from the dropper.")]
        [SerializeField]
        private float scatterDistance = 1f;

        [SerializeField]
        private DropLibrarySO dropLibrary;

        private const int ATTEMPS = 30;

        protected override Vector3 GetDropLocation()
        {
            // We might need to try more than once to get on the NavMesh
            for (int i = 0; i < ATTEMPS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * scatterDistance;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 0.1f, NavMesh.AllAreas))
                {
                    return hit.position;
                }
            }

            return transform.position;
        }

        #region Unity events
        public void RandomDrop()
        {
            BaseStats stat = GetComponent<BaseStats>();

            var drops = dropLibrary.GetRandomDrops(stat.CurrentLevel);
            foreach(var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }
        #endregion
    }
}
