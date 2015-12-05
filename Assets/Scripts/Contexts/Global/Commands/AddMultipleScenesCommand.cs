using System.Collections.Generic;
using System.Net;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Global.Commands {
    public class AddMultipleScenesCommand : Command {
        [Inject]
        public GameObject PreviousContext { get; set; }

        [Inject]
        public List<string> Scenes { get; set; }

        public override void Execute() {
            GameObject.Destroy(PreviousContext);
            foreach (var scene in Scenes) {
                UnityEngine.Application.LoadLevelAdditive(scene);
            }
        }
    }
}
