using Contexts.Global.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Global.Commands {
    public class FadeSceneBlackCommand : Command {
        [Inject]
        public ScreenFadedSignal ScreenFadedSignal { get; set; }

        [Inject]
        public FadeScreenSignal FadeScreenSignal { get; set; }
        
        public override void Execute() {
            Retain();
            ScreenFadedSignal.AddListener(OnComplete);
            FadeScreenSignal.Dispatch();
        }

        private void OnComplete() {
            ScreenFadedSignal.RemoveListener(OnComplete);
            Release();         
        }
    }
}
