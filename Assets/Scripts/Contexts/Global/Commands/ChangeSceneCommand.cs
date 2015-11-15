using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Global.Commands {
    public class ChangeSceneCommand : Command {
        [Inject]
        public GameObject PreviousContext { get; set; }
        
        [Inject]
        public string NextSceneName { get; set; }

        public override void Execute() {
            GameObject.Destroy(PreviousContext);
            UnityEngine.Application.LoadLevelAdditive(NextSceneName);
        }
    }
}
