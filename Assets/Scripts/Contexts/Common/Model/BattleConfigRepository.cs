﻿using System.Collections.Generic;

namespace Contexts.Common.Model {
    public class BattleConfigRepository : IBattleConfigRepository {

        private readonly List<IBattleConfig> _configs = new List<IBattleConfig>() {
            new BattleConfig(
                new Objectives.Rout(),
                "male_soldier_report"
            ),
            new BattleConfig(
                new Objectives.Survive(8),
                "chapter_2_intro"
            ),
        }; 

        public IBattleConfig GetConfigByIndex(int chapterIndex) {
            return _configs[chapterIndex];
        }
    }
}