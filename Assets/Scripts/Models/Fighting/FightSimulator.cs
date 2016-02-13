using System;
using Models.Combat;
using Models.Fighting.Equip;
using Models.Fighting.Skills;

namespace Models.Fighting {
    public class FightSimulator {
        private readonly IRandomizer _randomizer;
        private readonly IMap _map;

        public FightSimulator(IMap map) {
            _randomizer = new BasicRandomizer();
            _map = map;
        }

        public FightPreview Simulate(ICombatant attacker, ICombatant defender, ISkillStrategy skillStrategy) {
            var distance = MathUtils.ManhattanDistance(attacker.Position, defender.Position);
            var firstAttack = skillStrategy.Compute(attacker, defender, _randomizer);

            if (skillStrategy.SupportsFlanking) {
                var flankerPosition = MathUtils.GetPositionAcrossFight(attacker.Position);

                // TODO: Convert to ICombatant
                ICombatant flanker = null; //_map.GetUnitByPosition(flankerPosition);
                if (flanker != null) {
                    var flankStrategy = flanker.GetStrategyByDistance(distance);
                    var flankerAttack = flankStrategy.Compute(attacker, defender, _randomizer);
                }
            }

            var counterStrategy = defender.GetStrategyByDistance(distance);
            var counterAttack = counterStrategy.Compute(defender, attacker, _randomizer);
            if (skillStrategy.DidDouble(attacker, defender)) {
                var doubleAttack = skillStrategy.Compute(attacker, defender, _randomizer);
            }
            
            return null;
        }
        
        public FightPreview SimulatePrimaryWeapon(ICombatant attacker, ICombatant defender) {
            return null;
        }
        
        public FightPreview SimulateSecondaryWeapon(ICombatant attacker, ICombatant defender) {
            return null;
        }
        
        public FightPreview SimulateKinesis(ICombatant attacker, ICombatant defender) {
            return null;
        }
        
        public FightPreview SimulateHeal(ICombatant attacker, ICombatant defender) {
            return null;
        }
    }
}