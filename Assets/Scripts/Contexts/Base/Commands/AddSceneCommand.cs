using System;
using strange.extensions.command.impl;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Contexts.Base.Commands {
    public class AddSceneCommand : Command {
        [Inject]
        public string SceneName { get; set; }

        public override void Execute() {
            if (string.IsNullOrEmpty(SceneName)) {
                throw new Exception("Cannot load scene without name");
            }

            Debug.LogFormat("Loading Scene {0}", SceneName);
            SceneManager.LoadScene(SceneName, LoadSceneMode.Additive);
        }
    }
}
