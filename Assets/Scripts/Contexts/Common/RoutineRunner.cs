using System.Collections;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.Common {
    public class RoutineRunner : IRoutineRunner {
        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject ContextView { get; set; }

        private CoroutineRunnerBehavior runner;

        [PostConstruct]
        public void PostConstruct() {
            runner = ContextView.AddComponent<CoroutineRunnerBehavior>();
        }

        public Coroutine StartCoroutine(IEnumerator method) {
            return runner.StartCoroutine(method);
        }
    }

    public class CoroutineRunnerBehavior : MonoBehaviour {
    }
}