using System;
using System.Collections;
using System.Collections.Generic;
using Contexts.Battle.Signals;
using strange.extensions.command.impl;

namespace Contexts.Battle.Utilities {
    public static class EventHandlingUtils {
        public static void ProcessEvents(Command command, IEnumerable<IEnumerator> events,
            ProcessEventHandlersSignal processSignal, EventHandlersCompleteSignal completeSignal, Action onComplete) {
            
            command.Retain();
            Action action = null;
            action = new Action(() => {
                onComplete();
                command.Release();
                completeSignal.RemoveListener(action);
            });

            completeSignal.AddListener(action);
            processSignal.Dispatch(events);
        }
    }
}