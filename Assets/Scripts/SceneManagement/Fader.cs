using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

namespace RPG.SceneManagement {
    public class Fader : MonoBehaviour {

        private CanvasGroup canvasGroup;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediately() {
            canvasGroup.alpha = 1f;
        }

        public IEnumerator FadeOut(float fadeTime) {
            while (canvasGroup.alpha < 1) {
                canvasGroup.alpha += Time.deltaTime / fadeTime;
                yield return null;
            }
        }

        public IEnumerator FadeIn(float fadeTime) {
            while (canvasGroup.alpha > 0) {
                canvasGroup.alpha -= Time.deltaTime / fadeTime;
                yield return null;
            }
        }
    }
}
