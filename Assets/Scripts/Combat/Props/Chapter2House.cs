using System.Collections;
using Combat.Interactive.Rules;
using DG.Tweening;
using UnityEngine;

namespace Combat.Props {
    public class Chapter2House : MonoBehaviour, IToggleableProp {
        private TogglableTileRule _interactiveTileRule;
        private HouseLight[] _lights;

        private void Awake() {
            _interactiveTileRule = GetComponentInChildren<TogglableTileRule>();
            _lights = GetComponentsInChildren<HouseLight>();
        }

        public IEnumerator Enable() {
            // Turn on the lights
            // Make the door interactive
            var seq = DOTween.Sequence();
            foreach (var light in _lights) {
                var tween = light.GetEnableTween();
                seq.Insert(0, tween);
            }

            yield return seq.WaitForCompletion();

            _interactiveTileRule.Enabled = true;
        }

        public IEnumerator Disable() {
            // Kill the lights
            // Disable the interactivity
            var seq = DOTween.Sequence();
            foreach (var light in _lights) {
                var tween = light.GetDisableTween();
                seq.Insert(0, tween);
            }

            yield return seq.WaitForCompletion();

            _interactiveTileRule.Enabled = false;
        }
    }
}