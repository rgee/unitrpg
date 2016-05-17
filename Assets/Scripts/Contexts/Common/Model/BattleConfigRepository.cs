using System.Collections.Generic;
using Models.Fighting.Battle.Objectives;

namespace Contexts.Common.Model {
    public class BattleConfigRepository : IBattleConfigRepository {

        private readonly List<IBattleConfig> _configs = new List<IBattleConfig>() {
            new BattleConfig(
                new Rout(), 
                "chapter_1_battle_new",
                new List<string>() {
                    "Chapter 1/Intro/male_soldier_report",
                    "Chapter 1/Intro/female_soldier_report",
                    "Chapter 1/Intro/liat_janek_prep",
                    "Chapter 1/Intro/liat_audric_h2h",
                    "Chapter 1/Intro/liat_audric_balcony",
                    "Chapter 1/Intro/liat_audric_overlook"
                }
            ),
            new BattleConfig(
                new Survive(8), 
                "chapter_2_intro"
            ),
        }; 

        public IBattleConfig GetConfigByIndex(int chapterIndex) {
            return _configs[chapterIndex];
        }
    }
}