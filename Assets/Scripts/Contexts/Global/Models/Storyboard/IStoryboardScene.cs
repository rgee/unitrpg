using Contexts.Common.Model;
using Contexts.Global.Signals;
using UnityEngine;

namespace Assets.Scripts.Contexts.Global.Models {
    public interface IStoryboardScene {
        string Id { get; }
        void StartScene(GameObject source, ChangeSceneSignal changeSceneSignal, ApplicationState state);
    }
}