using UnityEngine;
using System.Collections;

public class CombatForecast : MonoBehaviour {

	GameObject attackerHalf;
	GameObject defenderHalf;

	// Use this for initialization
	void Start () {
		attackerHalf = transform.FindChild("Attacker").gameObject;
		defenderHalf = transform.FindChild("Defender").gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.N)) {
			attackerHalf.GetComponent<Animator>().SetTrigger("open");
			defenderHalf.GetComponent<Animator>().SetTrigger("open");
		}
	}
}
