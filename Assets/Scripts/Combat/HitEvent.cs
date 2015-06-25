using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Combat {
    public struct HitEvent {
        public Hit Data;
        public GameObject Target;
        public GameObject Attacker;
    }
}
