using DG.Tweening;
using UnityEngine;

namespace Combat.Props {
    public class HouseLight : MonoBehaviour {
        private static float ENABLE_TIME_SECONDS = 0.5f;
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

        public Tween GetEnableTween() {
            return _sprite.DOScale(_originalScale, ENABLE_TIME_SECONDS).SetEase(Ease.OutCubic);
        }

        public Tween GetDisableTween() {
            return _sprite.DOScale(new Vector3(0, 0, 1), ENABLE_TIME_SECONDS).SetEase(Ease.OutCubic);
        }
    }
}