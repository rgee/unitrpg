using System.Collections;
using Combat.Interactive.Rules;
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
            foreach (var light in _lights) {
                yield return StartCoroutine(light.Enable());
            }

            _interactiveTileRule.Enabled = true;
        }

        public IEnumerator Disable() {
            // Kill the lights
            // Disable the interactivity
            foreach (var light in _lights) {
                yield return StartCoroutine(light.Disable());
            }

            _interactiveTileRule.Enabled = false;
        }
    }
}