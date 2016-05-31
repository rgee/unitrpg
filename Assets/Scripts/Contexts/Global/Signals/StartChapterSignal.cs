using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Global.Signals {
    /// <summary>
    /// Provide the source root game object so the previous scene can be cleaned up.
    /// </summary>
    public class StartChapterSignal : Signal<GameObject> {
    }
}