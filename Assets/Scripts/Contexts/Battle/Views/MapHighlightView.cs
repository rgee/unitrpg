using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapHighlightView : View {
        private GameObject _hoverHighlight;

        void Awake() {
            base.Awake();
            _hoverHighlight = transform.FindChild("Hover").gameObject;
        }

        public void SetHighlightedPosition(Vector3 position) {
            _hoverHighlight.transform.position = position;
            _hoverHighlight.SetActive(true);
        }

        public void DisableHoverHighlight() {
            _hoverHighlight.SetActive(false);
        }

        public void EnableHoverHighlight() {
            _hoverHighlight.SetActive(true);
        }
    }
}