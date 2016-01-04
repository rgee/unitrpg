using DG.Tweening;
using UnityEngine;

namespace Combat.Props {
    public class HouseLight : MonoBehaviour {
        private static float ENABLE_TIME_SECONDS = 0.5f;
        private static float PULSE_SCALE_FACTOR = 0.3f;
        private static Vector3 PULSE_SCALE = new Vector3(PULSE_SCALE_FACTOR, PULSE_SCALE_FACTOR, 0);

        private tk2dSprite _sprite;
        private Vector3 _originalScale;
        private Tween _pulseTween;
        public bool Enabled;

        private void Awake() {
            _sprite = GetComponent<tk2dSprite>();
            _originalScale = _sprite.scale;
            CreatePulseTween();

            if (!Enabled) {
                _sprite.scale = new Vector3(0, 0, 1);
            } else {
                _pulseTween.Play();
            }

        }

        private void CreatePulseTween() {
            _pulseTween = _sprite.DOScale(PULSE_SCALE, 2f)
                .SetRelative()
                .SetLoops(-1, LoopType.Yoyo)
                .Pause();
        }

        public Tween GetEnableTween() {
            return _sprite.DOScale(_originalScale, ENABLE_TIME_SECONDS).SetEase(Ease.OutCubic);
        }

        public Tween GetDisableTween() {
            return _sprite.DOScale(new Vector3(0, 0, 1), ENABLE_TIME_SECONDS).SetEase(Ease.OutCubic);
        }

        public void EnablePulse() {
            CreatePulseTween();
            _pulseTween.Play();
        }

        public void DisablePulse() {
            _pulseTween.Kill();
        }
    }
}