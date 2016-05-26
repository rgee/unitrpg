using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using strange.extensions.mediation.impl;

namespace Contexts.Battle.Views.UniqueCombatants {
    public class LiatViewMediator : Mediator {
        [Inject]
        public LiatView View { get; set; }

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