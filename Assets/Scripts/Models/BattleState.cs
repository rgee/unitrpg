using System.Collections.Generic;
using UnityEngine;

namespace Models {
    public class BattleState {
        public Vector2 cameraPosition;
        public int turn = 1;
        public List<Combat.Unit> units;
    }
}