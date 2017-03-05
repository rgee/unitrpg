using System;
using Contexts.Common.Model;
using Contexts.Global.Signals;
using UnityEngine;

namespace Assets.Scripts.Contexts.Global.Models {
    public class StoryboardScene : IStoryboardScene {
        private readonly string _sceneName;
        private readonly string _id;
        public string Id { get { return _id;  } }

        public StoryboardScene(string id, string sceneName) {
            _id = id;
            _sceneName = sceneName;
        }

        public void StartScene(GameObject source, ChangeSceneSignal changeSceneSignal, ApplicationState state) {
            changeSceneSignal.Dispatch(source, _sceneName);
        }
    }
}