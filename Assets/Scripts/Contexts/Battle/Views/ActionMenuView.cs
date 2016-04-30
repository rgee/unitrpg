using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class ActionMenuView : View {
        public void Show(Vector3 position) {
            Debug.Log("Showing action menu at " + position);
        }

        public void Hide() {
            Debug.Log("Hiding action menu"); 
        }
    }
}