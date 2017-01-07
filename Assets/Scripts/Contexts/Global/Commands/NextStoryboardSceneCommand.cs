using Assets.Scripts.Contexts.Global.Models;
using Contexts.Common.Model;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Global.Commands {
    public class NextStoryboardSceneCommand : Command {
        [Inject] 
        public GameObject Source { get; set; }

        [Inject]
        public Storyboard Storyboard { get; set; }

        [Inject]
        public ApplicationState State { get; set; }

        [Inject]
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        public override void Execute() {
            var scene = Storyboard.GetAndIncrementNextScene();
            scene.StartScene(Source, ChangeSceneSignal, State);
        }
    }
}