using Models.Fighting.Effects;
using Models.Fighting.Maps;

namespace Models.Fighting.Skills {
    public class Knockback : MeleeAttack {
        private readonly IMap _map;

        public Knockback(IMap map) : base(SkillType.Knockback, true, true) {
            _map = map;
        }

        protected override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var baseEffects = base.ComputeEffects(forecast, randomizer);

            var attacker = forecast.Attacker;
            var defender = forecast.Defender;

            var direction = MathUtils.DirectionTo(attacker.Position, defender.Position);
            baseEffects.ReceiverEffects.Add(new Shove(direction, _map));

            return baseEffects;
        }
    }
}