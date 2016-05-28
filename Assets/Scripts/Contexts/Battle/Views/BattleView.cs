using System.Collections;
using System.Collections.Generic;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class BattleView : View {
        public Signal TriggerEventsCompleteSignal = new Signal();

        public void TriggerEventHandlers(IEnumerable<IEnumerator> handlerCoroutines) {
            StartCoroutine(ExecuteEventCoroutines(handlerCoroutines));
        }

        private IEnumerator ExecuteEventCoroutines(IEnumerable<IEnumerator> coroutines) {
            foreach (var coroutine in coroutines) {
                StartCoroutine(coroutine);
                yield return new WaitForSeconds(0.3f);
            }

            TriggerEventsCompleteSignal.Dispatch();
        }
    }
}