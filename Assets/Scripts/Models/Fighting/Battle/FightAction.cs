using Models.Fighting.Skills;

namespace Models.Fighting.Battle {
    public class FightAction : ICombatAction {
        private readonly ICombatant _attacker;
        private readonly ICombatant _defender;
        private readonly FightResult _fightResult;

        public bool IsValid(Turn turn) {
            return turn.CanAct(_attacker);
        }

        public void Perform(Turn turn) {
            // TODO: Break down the FightResult and apply the effects
        }
    }
}