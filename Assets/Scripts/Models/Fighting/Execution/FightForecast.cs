using Models.Fighting.Skills;

namespace Models.Fighting.Execution {
    public class FightForecast {
        public SkillForecast AttackerForecast { get; set; }
        public SkillForecast DefenderForecast { get; set; }
        public SkillForecast FlankerForecast { get; set; }
    }
}