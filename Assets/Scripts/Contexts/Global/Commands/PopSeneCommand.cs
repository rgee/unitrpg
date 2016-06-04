using Contexts.Common.Model;
using strange.extensions.command.impl;
using UnityEngine.SceneManagement;

namespace Contexts.Global.Commands {
    public class PopSeneCommand : Command {
        
        [Inject]
        public ApplicationState AppState { get; set; }
        
        public override void Execute() {
            var sceneName = AppState.AdditionalSceneStack.Pop();
            SceneManager.UnloadScene(sceneName);
        }
    }
}