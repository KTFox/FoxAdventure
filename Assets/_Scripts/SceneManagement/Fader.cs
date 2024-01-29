using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement {
    public class Fader : MonoBehaviour {

        private CanvasGroup canvasGroup;
        private Coroutine currentActiveFade;

        private void Awake() {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediately() {
            canvasGroup.alpha = 1f;
        }

        public Coroutine FadeOut(float fadeTime) {
            return Fade(1, fadeTime);
        }

        public Coroutine FadeIn(float fadeTime) {
            return Fade(0, fadeTime);
        }

        private Coroutine Fade(float target, float fadeTime) {
            if (currentActiveFade != null) {
                StopCoroutine(currentActiveFade);
            }

            currentActiveFade = StartCoroutine(FadeRoutine(target, fadeTime));
            return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float fadeTime) {
            while (!Mathf.Approximately(canvasGroup.alpha, target)) {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / fadeTime);
                yield return null;
            }
        }
    }
}
