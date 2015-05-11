using System.Collections;
using Grid;
using UnityEngine;

public class ShowingEXP : StateMachineBehaviour {
    private Animator Animator;
    private GridCameraController CameraController;
    private EXPBubble ExpBubble;
    private GameObject ExpPanel;
    public GameObject ExpPanelPrefab;
    private Grid.Unit SelectedUnit;
    private UnitManager UnitManager;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        UnitManager = CombatObjects.GetUnitManager();
        CameraController = CombatObjects.GetCameraController();
        Animator = animator;

        LockControls();

        ExpPanel = Instantiate(ExpPanelPrefab);

        SelectedUnit = CombatObjects.GetBattleState().SelectedUnit;

        var dialog = ExpPanel.GetComponent<EXPGainDialog>();

        var character = SelectedUnit.GetComponent<Grid.Unit>().GetCharacter();
        dialog.CharacterName = character.name;
        dialog.CharacterLevel = character.Level;
        dialog.StartingEXP = character.Exp;

        ExpBubble = ExpPanel.transform.FindChild("Panel/EXP Bubble").GetComponent<EXPBubble>();

        SelectedUnit.ApplyExp(50);
        ExpBubble.StartCoroutine(AnimateThenExit());
    }

    private IEnumerator AnimateThenExit() {
        yield return ExpBubble.StartCoroutine(ExpBubble.AnimateToExp(50, 0.7f));
        yield return new WaitForSeconds(2);

        if (SelectedUnit.CanLevel()) {
            Animator.SetTrigger("gained_level");
        } else {
            Animator.SetTrigger("no_exp");
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        Destroy(ExpPanel);
        UnlockControls();
    }

    private void LockControls() {
        UnitManager.Lock();
        CameraController.Lock();
    }

    private void UnlockControls() {
        UnitManager.Unlock();
        CameraController.Unlock();
    }
}