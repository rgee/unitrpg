using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    public class FullScreenFader : MonoBehaviour {
        public float FadeTime = .5f;
        private Image _image;

        void Awake() {
            _image = transform.Find("Image").GetComponent<Image>();
        }

        public IEnumerator FadeToBlack() {
            gameObject.SetActive(true);
            yield return _image.DOFade(1f, FadeTime).WaitForCompletion();
        }

        public IEnumerator Reveal() {
            yield return _image.DOFade(0f, FadeTime).WaitForCompletion();
            gameObject.SetActive(false);
        }
    }
}