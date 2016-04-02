namespace Models.Fighting.Skills {
    public class Advance : MeleeAttack {
        protected override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var baseResult = base.ComputeEffects(forecast, randomizer);

            var baseEffects = baseResult.ReceiverEffects;
            baseEffects.Add(new Effects.Advance(forecast.Attacker));

            return baseResult;
        }
    }
}