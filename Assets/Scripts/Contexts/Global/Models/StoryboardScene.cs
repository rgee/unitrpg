using Contexts.Common.Model;
using Contexts.Global.Signals;
using UnityEngine;

namespace Assets.Scripts.Contexts.Global.Models {
    public class StoryboardScene : IStoryboardScene {
        private readonly string _sceneName;

        public StoryboardScene(string sceneName) {
            _sceneName = sceneName;
        }

        public void StartScene(GameObject source, ChangeSceneSignal changeSceneSignal, ApplicationState state) {
            changeSceneSignal.Dispatch(source, _sceneName);
        }
    }
}