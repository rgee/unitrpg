﻿using Contexts.Battle.Models;
using Models.Combat;
using Models.Fighting.Skills;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class CombatActionSelectedCommand : Command {
        [Inject]
        public CombatActionType ActionType { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        public override void Execute() {
            if (ActionType == CombatActionType.Attack) {
                Model.State = BattleUIState.SelectingAttackTarget;
            } else if (ActionType == CombatActionType.Special) {
                Model.SpecialAttack = true;
                Model.State = BattleUIState.SelectingAttackTarget;
            } else if (ActionType == CombatActionType.Move) {
                Model.State = BattleUIState.SelectingMoveLocation;
            } else if (ActionType == CombatActionType.Fight) {
                Model.State = BattleUIState.SelectingFightAction;
            } else if (ActionType == CombatActionType.Use) {
                Model.State = BattleUIState.SelectingInteractTarget;
            } else {
                
            }
        }
    }
}
