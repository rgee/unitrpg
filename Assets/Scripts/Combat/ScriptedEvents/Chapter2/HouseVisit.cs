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

        public override IEnumerator Play() {

            // TODO: Play the dialogue

            var house = HouseObj.GetComponent<IToggleableProp>();
            var nextHouse = NextHouseObj.GetComponent<IToggleableProp>();

            yield return StartCoroutine(house.Disable());

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
    }
}