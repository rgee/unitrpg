using System;
using strange.extensions.signal.impl;

namespace Assets.Contexts.OverlayDialogue.Signals.Public {
    /// <summary>
    /// Start an overlay dialogue, loading the JSON from the given path.
    /// </summary>
    public class StartDialogueSignal : Signal<string> {
    }
}