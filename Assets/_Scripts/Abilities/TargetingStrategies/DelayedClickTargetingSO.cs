using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using RPG.Control;

namespace RPG.Abilities.TargetingStrategies
{
    [CreateAssetMenu(menuName = "ScriptableObject/TargetingStrategySO/DelayedClickTargetingSO")]
    public class DelayedClickTargetingSO : TargetingStrategySO
    {
        [SerializeField]
        private Texture2D cursorTexture;
        [SerializeField]
        private Vector3 cursorHotSpot;
        [SerializeField]
        private LayerMask affectedLayerMask;
        [SerializeField]
        private float areaAffectRadius;
        [SerializeField]
        private Transform targetingVisualPrefab;

        private Transform targetingVisualInstance;

        public override void StartTargeting(AbilityData data, Action finishTargeting)
        {
            PlayerController playerController = data.User.GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(data, playerController, finishTargeting));
        }

        private IEnumerator Targeting(AbilityData data, PlayerController playerController, Action finishTargeting)
        {
            playerController.enabled = false;

            // Set active targeting visual
            if (targetingVisualInstance == null)
            {
                targetingVisualInstance = Instantiate(targetingVisualPrefab);
            }
            else
            {
                targetingVisualInstance.gameObject.SetActive(true);
            }
            // The Squad has 0.5f radius by default. Therefore, it need to be doubly
            targetingVisualInstance.localScale = new Vector3(areaAffectRadius * 2, 1, areaAffectRadius * 2);

            while (true)
            {
                RaycastHit raycastHit;
                if (Physics.Raycast(PlayerController.GetMouseRay(), out raycastHit, 1000, affectedLayerMask))
                {
                    targetingVisualInstance.position = raycastHit.point;

                    if (Input.GetMouseButtonDown(0))
                    {
                        yield return new WaitWhile(() => Input.GetMouseButtonDown(0));

                        playerController.enabled = true;
                        targetingVisualInstance.gameObject.SetActive(false);
                        data.SetTargets(GetGameObjectsInRadius(raycastHit.point));
                        finishTargeting();
                        yield break;
                    }
                    yield return null;
                }
            }
        }

        private IEnumerable<GameObject> GetGameObjectsInRadius(Vector3 point)
        {
            RaycastHit[] hits = Physics.SphereCastAll(point, areaAffectRadius, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                yield return hit.collider.gameObject;
            }
        }
    }
}
