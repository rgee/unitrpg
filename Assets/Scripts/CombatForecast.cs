using UnityEngine;

public class CombatForecast : MonoBehaviour {
    private GameObject attackerHalf;
    private GameObject defenderHalf;
    private bool triggered;
    // Use this for initialization
    private void Start() {
        attackerHalf = transform.FindChild("Attacker").gameObject;
        defenderHalf = transform.FindChild("Defender").gameObject;
    }

    // Update is called once per frame
    private void Update() {
        if (!triggered && Input.GetKeyDown(KeyCode.N)) {
            attackerHalf.GetComponent<Animator>().SetTrigger("open_vertical");
            defenderHalf.GetComponent<Animator>().SetTrigger("open_vertical");
            triggered = true;
        }
    }
}