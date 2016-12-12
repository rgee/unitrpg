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

        public override void Execute() {
            var skillDatabase = new SkillDatabase(Model.Map);
            var finalizer = new FightFinalizer(skillDatabase);
            var finalizedFight = finalizer.Finalize(Model.FightForecast, new BasicRandomizer());

            var fightAction = new FightAction(finalizedFight);
            Model.PendingAction = fightAction; 
            Model.State = BattleUIState.Fighting;
        }
    }
}
