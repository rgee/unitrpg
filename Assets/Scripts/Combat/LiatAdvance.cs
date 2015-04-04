using UnityEngine;
using System.Collections;

public class LiatAdvance : MonoBehaviour {
	private Grid.Unit Unit;
	private Animator Animator;
	
	public void Start() {
		Unit = GetComponent<Grid.Unit>();
		Animator = GetComponent<Animator>();
	}

	public void Advance() {
		Vector3 advancePos = CombatObjects.GetBattleState().AttackTarget.transform.position;
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", advancePos,
			"time", 0.1f,
			"easetype", iTween.EaseType.easeOutExpo
		));
	}

	public void Update() {
		if (Unit.Attacking && Input.GetMouseButtonDown(0)) {
			Animator.SetTrigger("Special");
		}
	}
}
