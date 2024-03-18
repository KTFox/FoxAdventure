using System;
using UnityEngine;
using RPG.Control;

namespace RPG.Abilities.TargetingStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/Strategy/TargetingStrategy/DirectionalTargeting")]
    public class DirectionalTargetingSO : TargetingStrategySO
    {
        // Variables 

        [SerializeField]
        private LayerMask _affectedLayerMask;
        [SerializeField]
        private float _groundOffset;


        // Methods

        public override void StartTargeting(AbilityData abilityData, Action finishedCallback)
        {
            RaycastHit raycastHit;
            Ray ray = PlayerController.GetMouseRay();
            if (Physics.Raycast(ray, out raycastHit, 1000, _affectedLayerMask))
            {
                abilityData.TargetPoint = raycastHit.point + ray.direction * _groundOffset / ray.direction.y;
            }

            finishedCallback();
        }
    }
}
