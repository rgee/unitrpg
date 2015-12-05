using System.Collections;
using Contexts.Common.Utils;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Base.Commands {
    public class StartCommand : Command {
        [Inject]
        public IRoutineRunner RoutineRunner { get; set; }

        public override void Execute() {
            // Delay release of the Global Context's start sequence for a frame to
            // allow its bindings to be registered. Otherwise, dependent contexts
            // that depend on Global Context bindings will be unable to access them.
            Retain();
            RoutineRunner.StartCoroutine(Delay());
        }

        private IEnumerator Delay() {
            yield return new WaitForEndOfFrame();
            Release();
        }
    }
}
