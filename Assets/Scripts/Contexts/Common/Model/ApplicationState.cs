using System.Collections.Generic;

namespace Contexts.Common.Model {
    public class ApplicationState {
        public List<Models.Dialogue.Cutscene> CurrentCutsceneSequence { get; set; }
    }
}