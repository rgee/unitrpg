using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ScriptedEvents {
    public class SpawnableUnit : ScriptableObject {
        public GameObject Prefab;
        public Vector2 SpawnPoint;
    }
}
