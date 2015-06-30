using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Combat;
using UnityEngine;

namespace Grid {
    public abstract class Trigger : MonoBehaviour, ITrigger {
        public abstract IScriptedEvent Event { get; }
        public Vector2 Location { get; set; }
    }
}
