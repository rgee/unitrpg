using System.Collections.Generic;
using Assets.Contexts.Common.Services;
using Contexts.Common.Model;
using strange.extensions.command.impl;

namespace Contexts.Cutscene.Commands {
    public class LoadTestCutsceneCommand : Command {
        [Inject]
        public ApplicationState State { get; set; }

        [Inject]
        public ICutsceneLoader CutsceneLoader { get; set; }

        public override void Execute() {
            var cutscene = CutsceneLoader.Load("Chapter 1/Intro/male_soldier_report");
            State.CurrentCutsceneSequence = new List<Models.Dialogue.Cutscene> {cutscene};
        }
    }
}
