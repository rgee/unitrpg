using Contexts.Common.Model;
using strange.extensions.command.impl;
using UnityEngine.SceneManagement;

namespace Contexts.Global.Commands {
    public class PushSceneCommand : Command {
        [Inject]
        public string SceneName { get; set; }
        
        [Inject]
        public ApplicationState AppState { get; set; }

        public override void Execute() {
            AppState.AdditionalSceneStack.Push(SceneName);
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }
    }
}