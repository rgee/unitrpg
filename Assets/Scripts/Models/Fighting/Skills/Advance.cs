using System.Linq;
using Utils;

namespace Models.Fighting.Skills {
    public class Advance : AbstractSkillStrategy {
        private readonly MeleeAttack _melee;
        private readonly ProjectileAttack _projectile;

        public Advance(MeleeAttack melee, ProjectileAttack projectile) : base(SkillType.Advance, true, true) {
            _melee = melee;
            _projectile = projectile;
        }

        public override ICombatBuffProvider GetBuffProvider(ICombatant attacker, ICombatant defender) {
            return GetBaseStrategyByRange(attacker, defender)
                .GetBuffProvider(attacker, defender);
        }

        public override SkillEffects ComputeResult(ICombatant attacker, ICombatant defender, IRandomizer randomizer) {
            return GetBaseStrategyByRange(attacker, defender)
                .ComputeResult(attacker, defender, randomizer);
        }

        public override SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender) {
            return GetBaseStrategyByRange(attacker, defender)
                .ComputeForecast(attacker, defender);
        }

        private AbstractSkillStrategy GetBaseStrategyByRange(ICombatant attacker, ICombatant defender) {
            var range = MathUtils.ManhattanDistance(attacker.Position, defender.Position);
            if (range > 1 && HasRangedWeapon(attacker)) {
                return _projectile;
            }

            return _melee;
        }

        private bool HasRangedWeapon(ICombatant attacker) {
            return attacker.EquippedWeapons.Any(weapon => weapon.Range > 1);
        }

        public override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var baseResult = GetBaseStrategyByRange(forecast.Attacker, forecast.Defender)
                .ComputeEffects(forecast, randomizer);
            var damage = baseResult.GetDefenderDamage();
            if (damage >= forecast.Defender.Health) {
                var baseEffects = baseResult.ReceiverEffects;
                baseEffects.Add(new Effects.Advance(forecast.Attacker));
            }

            return baseResult;
        }
    }
}