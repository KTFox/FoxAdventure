using UnityEngine;
using UnityEngine.AI;
using RPG.Stats;

namespace RPG.Inventories
{
    public class RandomDropper : ItemDropper
    {
        private const int ATTEMPS = 30;

        [SerializeField]
        private float _scatterDistance = 1f;
        [SerializeField]
        private DropLibrarySO _dropLibrary;

        protected override Vector3 GetDropLocation()
        {
            for (int i = 0; i < ATTEMPS; i++)
            {
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * _scatterDistance;
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
            var baseStats = GetComponent<BaseStats>();
            var drops = _dropLibrary.GetRandomDrops(baseStats.CurrentLevel);

            foreach(var drop in drops)
            {
                DropItem(drop.item, drop.number);
            }
        }
        #endregion
    }
}
