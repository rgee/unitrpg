using System;
using System.Collections;
using System.Collections.Generic;
using Models.Combat;

namespace AI.Behavior {
    public class Sequence : Behavior {
    
        private List<Behavior> _behaviors;
        
        public Sequence(List<Behavior> behaviors) {
            this._behaviors = behaviors;
        }
        
        public override IEnumerator Act(Grid.Unit unit, IBattle battle)
        {
            foreach (var behavior in _behaviors) {
                yield return _monoBehaviour.StartCoroutine(behavior.Act(unit, battle));
                this.State = behavior.State;
            }
        }

        public override void Reset()
        {
            foreach (var behavior in _behaviors) {
                behavior.Reset();
            }
            
        }
    }   
}