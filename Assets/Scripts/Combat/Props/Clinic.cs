using System.Collections;
using UnityEngine;

namespace Combat.Props {
    public class Clinic : MonoBehaviour, IToggleableProp {
        public IEnumerator Enable() {
            // Turn on lights
            // Enable interactive tile
            yield return null;
        }

        public IEnumerator Disable() {
            yield return null;
        }
    }
}