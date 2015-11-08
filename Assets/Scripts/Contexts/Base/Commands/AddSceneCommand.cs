using System;
using strange.extensions.command.impl;

namespace Assets.Contexts.Application.Commands {
    public class AddSceneCommand : Command {
        [Inject]
        public string SceneName { get; set; }

        public override void Execute() {
            if (string.IsNullOrEmpty(SceneName)) {
                throw new Exception("Cannot load scene without name");
            }

            UnityEngine.Application.LoadLevelAdditive(SceneName);
        }
    }
}
