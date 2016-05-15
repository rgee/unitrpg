using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class CombatEffectsView : View {
        public GameObject CritConfirmPrefab;
        public GameObject GlanceConfirmPrefab;
        public GameObject HitConfirmPrefab;
        public GameObject SoundbankPrefab;

        public float CritIntensity = 6;
        public float RegularIntensity = 4;
        public float ShakeDecay = 0.1f;

        private SoundFX _fx;
        private float _shakeIntensity;
        private bool _shaking;
        private Vector3 _originalPos;

        private void Awake() {
            base.Awake();

            var soundbank = Instantiate(SoundbankPrefab);
            _fx = soundbank.GetComponent<SoundFX>();
        }

        private void Update() {
            if (_shakeIntensity > 0) {
                Vector3 offset = (Random.insideUnitCircle*_shakeIntensity);
                Camera.main.transform.position = _originalPos + new Vector3(offset.x, offset.y, 0);
                _shakeIntensity -= ShakeDecay;
            } else if (_shaking) {
                _shaking = false;
            }
        }

        public void ShowMiss() {
            _fx.PlayMiss();
        }

        public void ShowHit(Vector3 position) {
            var hitConfirmation = Instantiate(HitConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
            ShakeCamera();
            _fx.PlayHit();
        }

        public void ShowCrit(Vector3 position) {
            var hitConfirmation = Instantiate(CritConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
            CriticalShakeCamera();
            _fx.PlayCrit();
        }

        public void ShowGlance(Vector3 position) {
            var hitConfirmation = Instantiate(GlanceConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
            ShakeCamera();
            _fx.PlayGlance();
        }

        private void ShakeCamera() {
            _originalPos = Camera.main.transform.position;
            _shaking = true;
            _shakeIntensity = RegularIntensity;
        }

        private void CriticalShakeCamera() {
            _originalPos = Camera.main.transform.position;
            _shaking = true;
            _shakeIntensity = CritIntensity;
        }
    }
}