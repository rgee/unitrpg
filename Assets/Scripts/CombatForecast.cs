using UnityEngine;
using System.Collections;

public class CombatForecast : MonoBehaviour {

	GameObject attackerHalf;
	bool triggered = false;
	GameObject defenderHalf;

	// Use this for initialization
	void Start () {
		attackerHalf = transform.FindChild("Attacker").gameObject;
		defenderHalf = transform.FindChild("Defender").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (!triggered && Input.GetKeyDown(KeyCode.N)) {
			attackerHalf.GetComponent<Animator>().SetTrigger("open_vertical");
			defenderHalf.GetComponent<Animator>().SetTrigger("open_vertical");
			triggered = true;
		}
	}
}
