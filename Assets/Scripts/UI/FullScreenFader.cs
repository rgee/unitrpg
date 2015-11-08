using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI {
    [RequireComponent(typeof(Image))]
    public class FullScreenFader : MonoBehaviour {
        private Image _image;

        void Awake() {
            _image = GetComponent<Image>();
        }


        void Update() {
            _image.rectTransform.localScale = new Vector2(Screen.width, Screen.height);
        }

        IEnumerator FadeToBlack() {
            yield return _image.DOFade(1f, 0.3f).WaitForCompletion();
        }

        IEnumerator Reveal() {
            yield return _image.DOFade(0f, 0.3f).WaitForCompletion();
        }
    }
}