using DG.Tweening;
using UnityEngine;

public class LiatAdvance : MonoBehaviour {
    public float AdvanceTimeSeconds = 0.1f;
    private Animator Animator;
    private Grid.Unit Unit;

    public void Start() {
        Unit = GetComponent<Grid.Unit>();
        Animator = GetComponent<Animator>();
    }

    public void Advance() {
        var advancePos = CombatObjects.GetBattleState().AttackTarget.transform.position;
        transform.DOMove(advancePos, AdvanceTimeSeconds).SetEase(Ease.OutExpo);
    }

    public void Update() {
        if (Unit.Attacking && Unit.Killing && Input.GetMouseButtonDown(0)) {
            Animator.SetTrigger("Special");
        }
    }
}