using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class Knockback : MeleeAttack {
        public Knockback() : base(SkillType.Knockback, true, true) {
        }

        protected override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var baseEffects = base.ComputeEffects(forecast, randomizer);

            var attacker = forecast.Attacker;
            var defender = forecast.Defender;

            var direction = MathUtils.DirectionTo(attacker.Position, defender.Position);
            baseEffects.ReceiverEffects.Add(new Shove(direction));

            return baseEffects;
        }
    }
}