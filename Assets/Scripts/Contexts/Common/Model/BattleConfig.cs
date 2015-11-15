using System.Collections.Generic;
using Contexts.Common.Model.Objectives;

namespace Contexts.Common.Model {
    public class BattleConfig : IBattleConfig {
        public IObjective Objective { get; private set; }
        public string InitialSceneName { get; private set; }
        public List<string> DialogueResourceSequence { get; private set; }

        public BattleConfig(IObjective objective, string initialSceneName, List<string> dialogueResources) {
            Objective = objective;
            InitialSceneName = initialSceneName;
            DialogueResourceSequence = dialogueResources;
        }

        public BattleConfig(IObjective objective, string initialSceneName) {
            Objective = objective;
            InitialSceneName = initialSceneName;
            DialogueResourceSequence = new List<string>();
        }
    }
}