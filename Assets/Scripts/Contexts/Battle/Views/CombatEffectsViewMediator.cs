using System;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Models.Fighting.Effects;
using strange.extensions.mediation.impl;

namespace Contexts.Battle.Views {
    public class CombatEffectsViewMediator : Mediator {
        [Inject]
        public CombatEffectsView View { get; set; }

        [Inject]
        public AttackConnectedSignal AttackConnectedSignal { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }


        public override void OnRegister() {
            AttackConnectedSignal.AddListener(OnAttackConnected);
        }

        private void OnAttackConnected(WeaponHitConnection connection) {
            var dimensions = Model.Dimensions;
            var position = dimensions.GetWorldPositionForGridPosition(connection.Combatant.Position);
            switch (connection.Severity) {
                case WeaponHitSeverity.Normal:
                    View.ShowHit(position);
                    break;
                case WeaponHitSeverity.Crit:
                    View.ShowCrit(position);
                    break;
                case WeaponHitSeverity.Glance:
                    View.ShowGlance(position);
                    break;
            }
        }
    }
}