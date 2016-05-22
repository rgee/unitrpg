using System.Linq;
using Models.Fighting.Effects;
using Models.Fighting.Maps;

namespace Models.Fighting.Skills {
    public class Knockback : MeleeAttack {
        private readonly IMap _map;

        public Knockback(IMap map) : base(SkillType.Knockback, true, true) {
            _map = map;
        }

        protected override SkillForecast ComputeForecast(ICombatant attacker, ICombatant defender) {
            var result = base.ComputeForecast(attacker, defender);

            // Knockback only gets one hit, always
            var hit = new SkillHit {
                BaseDamage = DamageUtils.ComputeMeleeDamage(attacker, defender),
                HitCount = 1 
            };

            result.Hit = hit;

            return result;
        }

        protected override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var baseEffects = base.ComputeEffects(forecast, randomizer);

            var attacker = forecast.Attacker;
            var defender = forecast.Defender;

            if (!baseEffects.ReceiverEffects.OfType<Miss>().Any()) {
                var direction = MathUtils.DirectionTo(attacker.Position, defender.Position);
                baseEffects.ReceiverEffects.Add(new Shove(direction, _map));
            }

            return baseEffects;
        }
    }
}