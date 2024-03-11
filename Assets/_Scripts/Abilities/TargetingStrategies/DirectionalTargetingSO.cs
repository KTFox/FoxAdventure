using System;
using UnityEngine;
using RPG.Control;

namespace RPG.Abilities.TargetingStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/TargetingStrategySO/DirectionalTargetingSO")]
    public class DirectionalTargetingSO : TargetingStrategySO
    {
        [SerializeField]
        private LayerMask layerMask;
        [SerializeField]
        private float groundOffset;

        public override void StartTargeting(AbilityData data, Action finishTargeting)
        {
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit, 1000, layerMask))
            {
                data.SetTargetedPoint(raycastHit.point + ray.direction * groundOffset / ray.direction.y);
            }

            finishTargeting();
        }
    }
}
