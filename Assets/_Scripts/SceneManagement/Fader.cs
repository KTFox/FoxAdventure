using System.Collections;
using UnityEngine;

namespace RPG.SceneManagement
{
    public class Fader : MonoBehaviour
    {
        // Variables

        private CanvasGroup _canvasGroup;
        private Coroutine _currentActiveFade;


        // Methods

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediately()
        {
            _canvasGroup.alpha = 1f;
        }

        public Coroutine FadeOut(float fadeTime)
        {
            return Fade(1, fadeTime);
        }

        public Coroutine FadeIn(float fadeTime)
        {
            return Fade(0, fadeTime);
        }

        private Coroutine Fade(float targetGroupAlpha, float fadeTime)
        {
            if (_currentActiveFade != null)
            {
                StopCoroutine(_currentActiveFade);
            }

            _currentActiveFade = StartCoroutine(FadingCoroutine(targetGroupAlpha, fadeTime));

            return _currentActiveFade;
        }

        private IEnumerator FadingCoroutine(float targetGroupAlpha, float fadeTime)
        {
            while (!Mathf.Approximately(_canvasGroup.alpha, targetGroupAlpha))
            {
                _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, targetGroupAlpha, Time.unscaledTime / fadeTime);

                yield return null;
            }
        }
    }
}
