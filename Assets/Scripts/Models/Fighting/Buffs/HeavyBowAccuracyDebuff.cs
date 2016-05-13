using Models.Fighting.Skills;

namespace Models.Fighting.Buffs {
    public class HeavyBowAccuracyDebuff : AccuracyBuff {
        public HeavyBowAccuracyDebuff() : base(-20, "heavy_bow_accuracy_debuff") {
        }

        public override bool AppliesToSkill(SkillType type) {
            return type == SkillType.Ranged;
        }
    }
}