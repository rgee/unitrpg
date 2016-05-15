using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class CombatEffectsView : View {
        public GameObject CritConfirmPrefab;
        public GameObject GlanceConfirmPrefab;
        public GameObject HitConfirmPrefab;

        public void ShowHit(Vector3 position) {
            var hitConfirmation = Instantiate(HitConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
        }

        public void ShowCrit(Vector3 position) {
            var hitConfirmation = Instantiate(CritConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
        }

        public void ShowGlance(Vector3 position) {
            var hitConfirmation = Instantiate(GlanceConfirmPrefab);
            hitConfirmation.transform.localPosition = position;
        }
    }
}