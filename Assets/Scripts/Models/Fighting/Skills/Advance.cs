namespace Models.Fighting.Skills {
    public class Advance : MeleeAttack {
        public Advance() : base(SkillType.Advance, true, true) {
        }

        protected override SkillEffects ComputeEffects(SkillForecast forecast, IRandomizer randomizer) {
            var baseResult = base.ComputeEffects(forecast, randomizer);

            var damage = baseResult.GetDefenderDamage();
            if (damage >= forecast.Defender.Health) {
                var baseEffects = baseResult.ReceiverEffects;
                baseEffects.Add(new Effects.Advance(forecast.Attacker));
            }

            return baseResult;
        }
    }
}