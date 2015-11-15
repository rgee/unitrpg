using System.Collections.Generic;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Global.Signals {
    public class ChangeSceneMultiSignal : Signal<GameObject, List<string>> {
    }
}