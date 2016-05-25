using System.Linq;
using Models.Fighting.Effects;
using Models.Fighting.Maps;

namespace Models.Fighting.Skills {
    public class Knockback : MeleeAttack {
        private readonly IMap _map;

        public Knockback(IMap map) : base(SkillType.Knockback, true, true) {
            _map = map;
        }

        public override SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender) {
            var result = base.ComputeForecast(attacker, defender);

            // Knockback only gets one hit, always
            var hit = new SkillHit {
                BaseDamage = DamageUtils.ComputeMeleeDamage(attacker, defender),
                HitCount = 1 
            };

            result.Hit = hit;

            return result;
        }

        public override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var baseEffects = base.ComputeEffects(forecast, randomizer);

            var attacker = forecast.Attacker;
            var defender = forecast.Defender;

            // Knockback doesn't take effect if you miss
            if (!baseEffects.ReceiverEffects.OfType<Miss>().Any()) {
                var direction = MathUtils.DirectionTo(attacker.Position, defender.Position);
                var destination = MathUtils.GetAdjacentPoint(defender.Position, direction);

                // Knockback does nothing if the destination is blocked
                if (!_map.IsBlocked(destination)) {
                    baseEffects.ReceiverEffects.Add(new Shove(direction, _map));
                    baseEffects.ReceiverEffects.Add(new SuppressCounter());
                }
            }

            return baseEffects;
        }
    }
}