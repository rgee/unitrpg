using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Global.Commands {
    public class LoadSceneCommand : Command {
        [Inject]
        public string SceneName { get; set; }

        public override void Execute() {
            UnityEngine.Application.LoadLevel(SceneName);
        }
    }
}
