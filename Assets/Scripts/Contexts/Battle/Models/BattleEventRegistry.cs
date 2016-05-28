using System;
using System.Collections;
using System.Collections.Generic;

namespace Contexts.Battle.Models {
    /// <summary>
    /// Other contexts can hook into event tile events here.
    /// 
    /// For example in order to implement custom behavior in a chapter triggered by a tile
    /// you could bind a tile event handler here, and it will be called by the battle context when that tile is triggered.
    /// </summary>
    public class BattleEventRegistry {
        private readonly Dictionary<string, IEnumerator> _eventHandlers = new Dictionary<string, IEnumerator>();

        public IEnumerator GetHandler(string eventName) {
            if (_eventHandlers.ContainsKey(eventName)) {
                return _eventHandlers[eventName];
            }

            return DoNothing();
        }

        private IEnumerator DoNothing() {
            yield return null;
        }
    }
}