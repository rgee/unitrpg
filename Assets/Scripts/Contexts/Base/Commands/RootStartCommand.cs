using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Contexts.Base.Commands {
    public class RootStartCommand : Command {
        public override void Execute() {
            var globalSceneName = "Global";
            if (!SceneManager.GetSceneByName(globalSceneName).isLoaded) {
                SceneManager.LoadScene(globalSceneName, LoadSceneMode.Additive);
            } else {
                Debug.Log("Global load requested, but Global was already loaded.");
            }
        }
    }
}
