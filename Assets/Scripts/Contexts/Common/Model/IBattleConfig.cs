using System.Collections.Generic;
using JetBrains.Annotations;
using Models.Fighting.Battle.Objectives;

namespace Contexts.Common.Model {
    public interface IBattleConfig {
        IObjective Objective { get; }
        string InitialSceneName { get; }
        List<string> DialogueResourceSequence { get; } 
    }
}