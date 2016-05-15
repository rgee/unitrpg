using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class CombatEffectsView : View {
        public GameObject CritConfirmPrefab;
        public GameObject GlanceConfirmPrefab;
        public GameObject HitConfirmPrefab;

        public float CritIntensity = 6;
        public float RegularIntensity = 4;
        public float ShakeDecay = 0.1f;

        private float _shakeIntensity;
        private bool _shaking;
        private Vector3 _originalPos;

        private void Update() {
            if (_shakeIntensity > 0) {
                Vector3 offset = (Random.insideUnitCircle*_shakeIntensity);
                Camera.main.transform.position = _originalPos + new Vector3(offset.x, offset.y, 0);
                _shakeIntensity -= ShakeDecay;
            } else if (_shaking) {
                _shaking = false;
            }
        }

        public void ShowHit(Vector3 position) {
            var hitConfirmation = Instantiate(HitConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
            ShakeCamera();
        }

        public void ShowCrit(Vector3 position) {
            var hitConfirmation = Instantiate(CritConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
            CriticalShakeCamera();
        }

        public void ShowGlance(Vector3 position) {
            var hitConfirmation = Instantiate(GlanceConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
            ShakeCamera();
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