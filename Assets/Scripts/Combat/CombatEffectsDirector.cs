using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Combat {
    public class CombatEffectsDirector : MonoBehaviour {
        public GameObject CritConfirmPrefab;
        public GameObject GlanceConfirmPrefab;
        public GameObject HitConfirmPrefab;

        void Start() {
            CombatEventBus.HitEvents.AddListener(HandleHit);
        }

        void OnDestroy() {
            CombatEventBus.HitEvents.RemoveListener(HandleHit);
        }

        private void HandleHit(HitEvent hitEvent) {
            var target = hitEvent.Target;
            var hit = hitEvent.Data;
            if (hit.Crit) {
                ShowCrit(target);
            } else if (hit.Glanced) {
                ShowGlance(target);
            } else if (!hit.Missed) {
                ShowHit(target);
            }
        }

        private void ShowCrit(GameObject target) {
            var hitConfirmation = Instantiate(CritConfirmPrefab);
            hitConfirmation.transform.parent = target.transform;
            hitConfirmation.transform.localPosition = new Vector3();
        }

        private void ShowHit(GameObject target) {
            var hitConfirmation = Instantiate(HitConfirmPrefab);
            hitConfirmation.transform.parent = target.transform;
            hitConfirmation.transform.localPosition = new Vector3();
        }

        private void ShowGlance(GameObject target) {
            var hitConfirmation = Instantiate(GlanceConfirmPrefab);
            hitConfirmation.transform.parent = target.transform;
            hitConfirmation.transform.localPosition = new Vector3();
        }
    }
}
