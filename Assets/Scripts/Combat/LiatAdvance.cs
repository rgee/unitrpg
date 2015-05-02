using UnityEngine;

public class LiatAdvance : MonoBehaviour {
    private Animator Animator;
    private Grid.Unit Unit;

    public void Start() {
        Unit = GetComponent<Grid.Unit>();
        Animator = GetComponent<Animator>();
    }

    public void Advance() {
        var advancePos = CombatObjects.GetBattleState().AttackTarget.transform.position;
        iTween.MoveTo(gameObject, iTween.Hash(
            "position", advancePos,
            "time", 0.1f,
            "easetype", iTween.EaseType.easeOutExpo
            ));
    }

    public void Update() {
        if (Unit.Attacking && Unit.Killing && Input.GetMouseButtonDown(0)) {
            Animator.SetTrigger("Special");
        }
    }
}