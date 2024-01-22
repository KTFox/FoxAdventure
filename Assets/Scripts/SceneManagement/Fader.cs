using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

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

        public IEnumerator FadeOut(float fadeTime) {
            return Fade(1, fadeTime);
        }

        public IEnumerator FadeIn(float fadeTime) {
            return Fade(0, fadeTime);
        }

        private IEnumerator Fade(float target, float fadeTime) {
            if (currentActiveFade != null) {
                StopCoroutine(currentActiveFade);
            }

            currentActiveFade = StartCoroutine(FadeRoutine(target, fadeTime));
            yield return currentActiveFade;
        }

        private IEnumerator FadeRoutine(float target, float fadeTime) {
            while (!Mathf.Approximately(canvasGroup.alpha, target)) {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, target, Time.deltaTime / fadeTime);
                yield return null;
            }
        }
    }
}
