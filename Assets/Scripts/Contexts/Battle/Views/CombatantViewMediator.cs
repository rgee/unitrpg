using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Models.Fighting.Effects;
using strange.extensions.mediation.impl;

namespace Contexts.Battle.Views {
    public class CombatantViewMediator : Mediator {
        [Inject]
        public CombatantView View { get; set; }

        [Inject]
        public BattleViewState BattleModel { get; set; }

        [Inject]
        public AttackConnectedSignal AttackConnectedSignal { get; set; }

        public override void OnRegister() {
            View.AttackConnectedSignal.AddListener(OnAttackConnected);
        }

        private void OnAttackConnected(WeaponHitConnection hitConnection) {
            AttackConnectedSignal.Dispatch(hitConnection);
        }
    }
}