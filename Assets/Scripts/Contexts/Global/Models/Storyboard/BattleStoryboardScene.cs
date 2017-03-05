using Contexts.Common.Model;
using Contexts.Global.Signals;
using Models.Fighting.Battle.Objectives;
using UnityEngine;

namespace Assets.Scripts.Contexts.Global.Models {
    public class BattleStoryboardScene : IStoryboardScene {
        public string Id { get; private set; }
        public IObjective Objective { get; private set; }
        private readonly string _sceneName;

        public BattleStoryboardScene(string id, string sceneName, IObjective objective) {
            Id = id;
            _sceneName = sceneName;
            Objective = objective;
        }

        public void StartScene(GameObject source, ChangeSceneSignal changeSceneSignal, ApplicationState state) {
            changeSceneSignal.Dispatch(source, _sceneName);
        }
    }
}