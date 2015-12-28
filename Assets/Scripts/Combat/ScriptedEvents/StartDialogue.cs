using System.Collections;
using Combat;
using UnityEngine;

namespace ScriptedEvents {
    public class StartDialogue : MonoBehaviour, IScriptedEvent {
        public GameObject DialoguePrefab;

        public IEnumerator Play() {
            var dialogueObject = Instantiate(DialoguePrefab);
            var dialogue = dialogueObject.GetComponent<Dialogue>();

            var completed = false;
            dialogue.OnComplete += () => {
                completed = true;
            };

            dialogue.Begin();
            while (!completed) {
                yield return null;
            }

            Destroy(dialogueObject);
        }
    }
}