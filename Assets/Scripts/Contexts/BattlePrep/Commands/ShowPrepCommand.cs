using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.BattlePrep.Commands {
    public class ShowPrepCommand : Command {
        public override void Execute() {
            Debug.Log("Prep start");
        }
    }
}
