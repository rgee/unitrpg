using Models.Combat;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class ActionMenuView : View {
        [Tooltip("How far from the center the bubbles should be.")]
        public float Scale = 10f;

        [Tooltip("The prefab for each bubble.")]
        public GameObject BubblePrefab;

        /// <summary>
        /// Dispatched the user clicks a bubble for an action.
        /// </summary>
        public Signal<CombatActionType> ActionTypeSelectedSignal = new Signal<CombatActionType>(); 

        /// <summary>
        /// Dispatched when the user clicks the top level cancelation button.
        /// </summary>
        public Signal CancelSignal = new Signal();

        /// <summary>
        /// Dispatched when the user clicks the Fight menu back button.
        /// </summary>
        public Signal BackSignal = new Signal();

        /// <summary>
        /// Dispatched when the user clicks to enter the fight menu.
        /// </summary>
        public Signal FightSignal = new Signal();

        public void ShowFightSubMenu() {
            
        }

        public void ReturnToTop() {
            
        }

        public void Show(Vector3 position) {
            Debug.Log("Showing action menu at " + position);
        }

        public void Hide() {
            Debug.Log("Hiding action menu"); 
        }
    }
}