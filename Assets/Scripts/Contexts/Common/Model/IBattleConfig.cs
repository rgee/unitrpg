using System.Collections.Generic;
using Contexts.Common.Model.Objectives;
using JetBrains.Annotations;

namespace Contexts.Common.Model {
    public interface IBattleConfig {
        IObjective Objective { get; }
        string InitialSceneName { get; }
        List<string> DialogueResourceSequence { get; } 
    }
}