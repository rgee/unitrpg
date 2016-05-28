using System.Collections;
using Contexts.Battle.Models;
using UnityEngine;

namespace Assets.Contexts.Chapters.EventTesting {
    public class DummyEventLoggerService {
        [Inject] 
        public BattleEventRegistry EventRegistry { get; set; }

        [PostConstruct]
        public void RegisterEvents() {
            EventRegistry.RegisterHandler("test", _logEvent());
        }

        private IEnumerator _logEvent() {
            Debug.Log("received event! Waiting three seconds.");
            for (var i = 0; i < 3; i++) {
                Debug.Log(i);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}