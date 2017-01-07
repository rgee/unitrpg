using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Contexts.Global.Commands {
    public class ChangeSceneCommand : Command {
        [Inject]
        public GameObject PreviousContext { get; set; }
        
        [Inject]
        public string NextSceneName { get; set; }

        public override void Execute() {
            SceneManager.LoadScene(NextSceneName, LoadSceneMode.Additive);
            GameObject.Destroy(PreviousContext);
        }
    }
}
