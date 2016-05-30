using System.Linq;
using Models.Fighting.Characters;
using Models.Fighting.Maps;
using Models.Fighting.Skills;
using Utils;

namespace Models.Fighting.Execution {
    public class FightForecaster {
        private readonly IMap _map;
        private readonly SkillDatabase _skillDatabase;

        public FightForecaster(IMap map, SkillDatabase skillDatabase) {
            _map = map;
            _skillDatabase = skillDatabase;
        }

        public FightForecast Forecast(ICombatant attacker, ICombatant defender, SkillType type) {
            var strategy = _skillDatabase.GetStrategyByType(type);
            var firstSkill = strategy.Forecast(attacker, defender);
            var flankForecast = null as SkillForecast;

            if (strategy.SupportsFlanking) {
                var direction = MathUtils.DirectionTo(attacker.Position, defender.Position);
                var flankerPosition = MathUtils.GetPositionAcrossFight(attacker.Position, direction);
                var flankerDistance = MathUtils.ManhattanDistance(defender.Position, flankerPosition);

                var flanker = _map.GetAtPosition(flankerPosition);
                if (flanker != null && flanker.Army == ArmyType.Friendly) {
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