using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using strange.extensions.mediation.impl;

namespace Contexts.Battle.Views.UniqueCombatants {
    // StrangeIoC does not work when binding two views to the same mediator class.
    // Nor does it allow subclassing the mediator to bind it. So far the 
    // only way I've gotten this to work is by duplication.
    public class JanekViewMediator : Mediator {
         
        [Inject]
        public JanekView View { get; set; }

        [Inject]
        public AttackConnectedSignal AttackConnectedSignal { get; set; }

        public override void OnRegister() {
            
            View.AttackConnectedSignal.AddListener(OnAttackConnected);
            View.DeathSignal.AddListener(OnDeath);
        }

        private void OnAttackConnected(WeaponHitConnection hitConnection) {
            AttackConnectedSignal.Dispatch(hitConnection);
        }

        private void OnDeath() {
            Destroy(View.gameObject);
        }
    }
}