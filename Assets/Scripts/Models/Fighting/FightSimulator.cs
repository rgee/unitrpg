using Models.Fighting.Skills;

namespace Models.Fighting {
    public class FightSimulator {
        private readonly IRandomizer _randomizer;

        public FightSimulator() {
            _randomizer = new BasicRandomizer();
        }

        public FightPreview Simulate(ICombatant attacker, ICombatant defender, ICombatant flanker, ISkillStrategy attackerStrategy,
            ISkillStrategy flankerStrategy, ISkillStrategy defenderStrategy) {

            var distance = MathUtils.ManhattanDistance(attacker.Position, defender.Position);
            var firstAttack = attackerStrategy.Compute(attacker, defender, _randomizer);
            var flankerAttack = null as SkillResult;
            var doubleAttack = null as SkillResult;

            if (flankerStrategy != null && flanker != null) {
                flankerAttack = flankerStrategy.Compute(flanker, defender, _randomizer);
            }

            var counterStrategy = defender.GetStrategyByDistance(distance);
            var counterAttack = null as SkillResult;
            if (counterStrategy != null) {
                counterAttack = counterStrategy.Compute(defender, attacker, _randomizer);
            }

            if (attackerStrategy.DidDouble(attacker, defender)) {
                doubleAttack = attackerStrategy.Compute(attacker, defender, _randomizer);
            }

            // Defender dies on the first hit
            var defenderHealth = defender.Health;
            var firstAttackDamage = firstAttack.GetDefenderDamage();
            var totalDamage = firstAttackDamage;
            if (firstAttackDamage >= defenderHealth) {
               // Bail out with just the first attack 
                return new FightPreview {
                    Initial =  firstAttack
                };
            }

            if (flankerAttack != null) {
                var flankDamage = flankerAttack.GetDefenderDamage();
                totalDamage += flankDamage;
                if (totalDamage >= defenderHealth) {
                    // Bail out with two attacks
                    return new FightPreview {
                        Initial = firstAttack,
                        Flank = flankerAttack
                    };
                }
            }

            var attackerHealth = attacker.Health;
            var totalAttackerDamage = 0;
            if (counterAttack != null) {
                // Attacker dies after being countered
                totalAttackerDamage = counterAttack.GetDefenderDamage();
                if (totalAttackerDamage >= attackerHealth) {
                    // Bail out with 1-2 initial attacks and the defender attack
                    return new FightPreview {
                        Initial = firstAttack,
                        Flank = flankerAttack,
                        Counter = counterAttack
                    };
                }
            }

            if (doubleAttack != null) {
                var doubleDamage = doubleAttack.GetDefenderDamage();
                totalAttackerDamage += doubleDamage;
                if (totalAttackerDamage >= attackerHealth) {
                    // Bail out with all previous hits + the double
                    return new FightPreview {
                        Initial = firstAttack,
                        Flank = flankerAttack,
                        Counter = counterAttack,
                        Double = doubleAttack
                    };
                }
            }

            // Bail out with just the first hit
            return null;
        } 

        public FightPreview Simulate(ICombatant attacker, ICombatant defender, ISkillStrategy skillStrategy) {
            var distance = MathUtils.ManhattanDistance(attacker.Position, defender.Position);
            var flankerStrategy = null as ISkillStrategy;
            var flanker = null as ICombatant;

            if (skillStrategy.SupportsFlanking) {
                var flankerPosition = MathUtils.GetPositionAcrossFight(attacker.Position);

                // TODO: Convert to ICombatant
                flanker = null; //_map.GetUnitByPosition(flankerPosition);
                if (flanker != null) {
                    flankerStrategy = flanker.GetStrategyByDistance(distance);
                }
            }

            var counterStrategy = defender.GetStrategyByDistance(distance);

            return Simulate(attacker, defender, flanker, skillStrategy, flankerStrategy, counterStrategy);
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