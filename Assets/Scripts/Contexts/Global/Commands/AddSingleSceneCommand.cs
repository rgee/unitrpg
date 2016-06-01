using strange.extensions.command.impl;
using UnityEngine.SceneManagement;

namespace Contexts.Global.Commands {
    public class AddSingleSceneCommand : Command {
        [Inject]
        public string SceneName { get; set; }

        public override void Execute() {
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }
    }
}
