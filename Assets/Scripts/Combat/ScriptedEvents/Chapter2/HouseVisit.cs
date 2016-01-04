using System.Collections;
using Combat;
using Combat.Interactive.Rules;
using Combat.Managers;
using Combat.Props;
using UnityEngine;

namespace Assets.Combat.ScriptedEvents.Chapter2 {
    [RequireComponent(typeof(TogglableTileRule))]
    public class HouseVisit : CombatEvent {
        public Chapter2Manager ChapterManager;
        public GameObject HouseObj; 
        public GameObject NextHouseObj;

        public GameObject LiatDialoguePrefab;
        public GameObject JanekDialoguePrefab;

        public GameObject LiatRejectedDialoguePrefab;
        public GameObject JanekRejectedDialoguePrefab;

        public override IEnumerator Play() {

            yield return StartCoroutine(PlayDialogue());

            var house = HouseObj.GetComponent<IToggleableProp>();
            yield return StartCoroutine(house.Disable());

            yield return StartCoroutine(RunRejectedDialogue());

            var nextHouse = NextHouseObj.GetComponent<IToggleableProp>();
            if (nextHouse != null) {
                var originalCameraPosition = CombatObjects.GetCamera().transform.position;
                var newHouseTransform = NextHouseObj.transform;
                var housePosition = new Vector3(newHouseTransform.position.x, newHouseTransform.position.y, originalCameraPosition.z);

                yield return new WaitForSeconds(0.3f);
                yield return StartCoroutine(PanCamera(housePosition));
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(nextHouse.Enable());
                yield return new WaitForSeconds(0.1f);
                yield return StartCoroutine(PanCamera(originalCameraPosition));
            }
        }

        private IEnumerator PlayDialogue() {
            var selectedUnitName = CombatObjects.GetBattleState().SelectedUnit.model.Character.Name;
            GameObject prefab;
            if (selectedUnitName.Equals("Liat")) {
                prefab = LiatDialoguePrefab;
            } else {
                prefab = JanekDialoguePrefab;
            }

            yield return StartCoroutine(RunDialogue(prefab));
        }

        private IEnumerator RunRejectedDialogue() {
            
            var selectedUnitName = CombatObjects.GetBattleState().SelectedUnit.model.Character.Name;
            GameObject prefab;
            if (selectedUnitName.Equals("Liat")) {
                prefab = LiatRejectedDialoguePrefab;
            } else {
                prefab = JanekRejectedDialoguePrefab;
            }

            yield return StartCoroutine(RunDialogue(prefab));
        }

        private IEnumerator RunDialogue(GameObject prefab) {
            var dialogueObject = Instantiate(prefab);
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