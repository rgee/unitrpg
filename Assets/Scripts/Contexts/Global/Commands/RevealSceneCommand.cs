using Contexts.Global.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Global.Commands {
    public class RevealSceneCommand : Command {
        [Inject]
        public ScreenRevealedSignal ScreenRevealedSignal { get; set; }

        [Inject]
        public RevealScreenSignal RevealScreenSignal { get; set; }

        public override void Execute() {
            Retain();
            ScreenRevealedSignal.AddListener(OnComplete);

            RevealScreenSignal.Dispatch();
        }

        private void OnComplete() {
            ScreenRevealedSignal.RemoveListener(OnComplete);
            Release();
        }
    }
}
