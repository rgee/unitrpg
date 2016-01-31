
using System.Collections;
using Models.Combat;
using UnityEngine;

namespace AI.Behavior {
    public abstract class Behavior {
        public enum BehaviorState {
            Success,
            Running,
            Failure
        };
        
        protected MonoBehaviour _monoBehaviour;
        public BehaviorState State { get; protected set;}
        
        public abstract IEnumerator Act(Grid.Unit unit, IBattle battle);
        public abstract void Reset();
        
        protected void _succeed() {
            this.State = BehaviorState.Success;
        }
        
        protected void _fail() {
            this.State = BehaviorState.Failure;
        }
        
        public bool _isRunning() {
            return this.State == BehaviorState.Running;
        }
        
        public bool _isFailed() {
            return this.State == BehaviorState.Failure;
        }
        
        public bool _isSuccessful() {
            return this.State == BehaviorState.Success;
        }
    }
}