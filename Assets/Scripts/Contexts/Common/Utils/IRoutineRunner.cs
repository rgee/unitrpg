using System.Collections;
using UnityEngine;

namespace Contexts.Common.Utils {
    public interface IRoutineRunner {
        Coroutine StartCoroutine(IEnumerator method);
    }
}