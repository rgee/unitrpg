using Contexts.Global.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Cutscene.Commands {
    public class StartDialogueCommand : Command {
        public override void Execute() {
            Debug.Log("Starting dialogue");
        }
    }
}
