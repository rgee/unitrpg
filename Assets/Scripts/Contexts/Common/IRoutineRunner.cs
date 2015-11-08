using System.Collections;
using UnityEngine;

namespace Contexts.Common {
    public interface IRoutineRunner {
        Coroutine StartCoroutine(IEnumerator method);
    }
}