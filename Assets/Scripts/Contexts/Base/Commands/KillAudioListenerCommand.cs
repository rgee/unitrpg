using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.Base.Commands {
    public class KillAudioListenerCommand : Command {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject ContextView { get; set; }

        public override void Execute() {

            var listeners = ContextView.GetComponentsInChildren<AudioListener>();
            foreach (var audioListener in listeners) {
                audioListener.enabled = false;
            }
        }
    }
}
