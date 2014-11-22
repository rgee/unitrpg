using UnityEngine;
using System.Collections;

public class PlayerPhaseText : MonoBehaviour {

	public BattleManager battleManager;

	public void TriggerPlayerPhase() {
		battleManager.StartPlayerPhase();
	}
}
