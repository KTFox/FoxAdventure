using System.Collections.Generic;
using UnityEngine;

namespace RPG.Abilities
{
    public abstract class FilterStrategySO : ScriptableObject
    {
        public abstract IEnumerable<GameObject> GetFilteredGameObjects(IEnumerable<GameObject> objectsToFilter);
    }
}
