using Contexts.Common.Model.Objectives;

namespace Contexts.Common.Model {
    public class BattleConfig : IBattleConfig {
        public IObjective Objective { get; private set; }
        public string InitialSceneName { get; private set; }

        public BattleConfig(IObjective objective, string initialSceneName) {
            Objective = objective;
            InitialSceneName = initialSceneName;
        }
    }
}