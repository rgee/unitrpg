using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Models.Combat;
using Models.Fighting.Skills;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class CombatActionSelectedCommand : Command {
        [Inject]
        public CombatActionType ActionType { get; set; }

        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public MoveSelectedSignal MoveSelectedSignal { get; set; }

        public override void Execute() {
            if (ActionType == CombatActionType.Attack) {
                // TODO: Check if we should use melee or ranged.
                Model.SelectedSkillType = SkillType.Melee;
                Model.State = BattleUIState.SelectingAttackTarget;
            } else if (ActionType == CombatActionType.Speical) {
                // TODO: Determine special skill type
            } else if (ActionType == CombatActionType.Move) {
                Model.State = BattleUIState.SelectingMoveLocation;
                // TODO: Get move range and set highlighted squares
                MoveSelectedSignal.Dispatch();
            } else if (ActionType == CombatActionType.Fight) { 
                Model.State = BattleUIState.SelectingFightAction;
            } else {
                
            }
        }
    }
}
