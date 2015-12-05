using Contexts.Cutscene.Signals;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Cutscene.Commands {
    public class StartDialogueCommand : Command {
        [Inject]
        public StartCutsceneSignal StartCutsceneSignal { get; set; }

        public override void Execute() {
            StartCutsceneSignal.Dispatch();
        }
    }
}
