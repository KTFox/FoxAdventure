using System.Collections;
using UnityEngine;
using RPG.Control;

namespace RPG.Abilities.Targeting
{
    [CreateAssetMenu(menuName = "ScriptableObject/TargetingStrategySO/DelayedClickTargetingSO")]
    public class DelayedClickTargetingSO : TargetingStrategySO
    {
        [SerializeField]
        private Texture2D cursorTexture;
        [SerializeField]
        private Vector3 cursorHotSpot;

        public override void StartTargeting(GameObject user)
        {
            PlayerController playerController = user.GetComponent<PlayerController>();
            playerController.StartCoroutine(Targeting(playerController));
        }

        private IEnumerator Targeting(PlayerController playerController)
        {
            playerController.enabled = false;

            while (true)
            {
                Cursor.SetCursor(cursorTexture, cursorHotSpot, CursorMode.Auto);

                if (Input.GetMouseButtonDown(0))
                {
                    yield return new WaitWhile(() => Input.GetMouseButtonDown(0));

                    playerController.enabled = true;

                    yield break;
                }
                yield return null;
            }
        }
    }
}
