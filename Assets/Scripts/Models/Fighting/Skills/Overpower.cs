using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class Overpower : MeleeAttack {
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