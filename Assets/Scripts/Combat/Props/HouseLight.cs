using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Combat.Props {
    public class HouseLight : MonoBehaviour, IToggleableProp {
        private tk2dSprite _sprite;
        private Vector3 _originalScale;
        public bool Enabled;

        private void Awake() {
            _sprite = GetComponent<tk2dSprite>();
            _originalScale = _sprite.scale;

            if (!Enabled) {
                _sprite.scale = new Vector3(0, 0, 1);
            }
        }

        public IEnumerator Enable() {
            yield return _sprite.DOScale(_originalScale, 0.7f).WaitForCompletion();
            Enabled = true;
        }

        public IEnumerator Disable() {
            yield return _sprite.DOScale(new Vector3(0, 0, 1), 0.7f).WaitForCompletion();
            Enabled = false;
        }
    }
}