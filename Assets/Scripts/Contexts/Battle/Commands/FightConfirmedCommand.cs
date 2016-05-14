using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Execution;
using Models.Fighting.Skills;
using strange.extensions.command.impl;

namespace Contexts.Battle.Commands {
    public class FightConfirmedCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        [Inject]
        public NewFinalizedFightSignal FinalizedFightSignal { get; set; }

        public override void Execute() {
            var attacker = Model.SelectedCombatant;
            var defender = Model.SelectedTarget;
            var skillDatabase = new SkillDatabase(Model.Map);
            var finalizer = new FightFinalizer(skillDatabase);
            var finalizedFight = finalizer.Finalize(Model.FightForecast, new BasicRandomizer());
            var action = new FightAction(attacker, defender, finalizedFight);

            Model.Battle.SubmitAction(action);

            FinalizedFightSignal.Dispatch(finalizedFight);
            Model.State = BattleUIState.Fighting;
        }
    }
}
