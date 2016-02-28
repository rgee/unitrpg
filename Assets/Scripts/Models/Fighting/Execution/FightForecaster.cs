using System.Linq;
using Models.Fighting.Skills;

namespace Models.Fighting.Execution {
    public class FightForecaster {
        public FightForecast Forecast(ICombatant attacker, ICombatant defender, SkillType type) {
            var strategy = SkillDatabase.Instance.GetStrategyByType(type);
            var firstSkill = strategy.Forecast(attacker, defender);
            var flankForecast = null as SkillForecast;

            if (strategy.SupportsFlanking) {
                var flankerPosition = MathUtils.GetPositionAcrossFight(attacker.Position);
                var flankerDistance = MathUtils.ManhattanDistance(defender.Position, flankerPosition);

                // TODO: Convert to ICombatant
                var flanker = null as ICombatant; //_map.GetUnitByPosition(flankerPosition);
                if (flanker != null) {
                    var flankerStrategy = ChooseStrategyByDistance(flanker, flankerDistance);
                    flankForecast = flankerStrategy.Forecast(flanker, defender);
                }
            }

            var distance = MathUtils.ManhattanDistance(attacker.Position, defender.Position);
            var counterStrategy = ChooseStrategyByDistance(defender, distance);
            var defenderforecast = null as SkillForecast;
            if (counterStrategy != null) {
                defenderforecast = counterStrategy.Forecast(defender, attacker);
            }

            return new FightForecast {
                AttackerForecast = firstSkill,
                DefenderForecast = defenderforecast,
                FlankerForecast = flankForecast
            };
        }

        private ISkillStrategy ChooseStrategyByDistance(ICombatant combatant, int distance) {
            var appropriateWeapon = combatant.EquippedWeapons.FirstOrDefault(weapon => weapon.Range == distance);

            if (appropriateWeapon == null) {
                return null;
            }

            if (distance == 1) {
                return new MeleeAttack();
            } else {
                return new ProjectileAttack();
            }
        }
    }
}