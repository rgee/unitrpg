using Assets.Contexts.Common.Services;
using Contexts.Common.Model;
using Contexts.Global.Signals;
using UnityEngine;

namespace Assets.Scripts.Contexts.Global.Models {
    public class Cutscene : IStoryboardScene {
        private readonly string _cutsceneSceneName;
        private readonly ICutsceneLoader _cutsceneLoader;
        private readonly string _cutsceneResourcePath;

        public Cutscene(ICutsceneLoader cutsceneLoader, string cutsceneSceneName, string cutsceneResourcePath) {
            _cutsceneLoader = cutsceneLoader;
            _cutsceneSceneName = cutsceneSceneName;
            _cutsceneResourcePath = cutsceneResourcePath;
        }

        public void StartScene(GameObject source, ChangeSceneSignal changeSceneSignal, ApplicationState state) {
            var parsedCutscene = _cutsceneLoader.Load(_cutsceneResourcePath);
            state.CurrentCutscene = parsedCutscene; 
            changeSceneSignal.Dispatch(source, _cutsceneSceneName);
        }
    }
}