using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Models {
    
    public class BattleState {
        public List<Unit> units;
        public int turn = 1;
        public Vector2 cameraPosition;
    }
}