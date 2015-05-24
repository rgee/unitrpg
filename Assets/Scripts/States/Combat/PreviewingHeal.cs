using UnityEngine;
using Models.Combat.Inventory;

public class PreviewingHeal : StateMachineBehaviour {
    private HealPreviewManager _previewManager;
    private Animator _animator;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        _animator = animator;
        _previewManager = CombatObjects.GetHealPreviewManager();
        _previewManager.ShowHealPreview().OnConfirm += DoHeal;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            CombatObjects.GetBattleState().SelectedItem = null;
            _previewManager.HideHealPreview();
            animator.SetTrigger("action_canceled");
        }
    }

    private void DoHeal() {
        BattleState state = CombatObjects.GetBattleState();
        Item item = state.SelectedItem;
        Models.Combat.Unit unitModel = state.SelectedUnit.model;

        CombatObjects.GetBattleState().Model.UseItem(item, unitModel);

        _previewManager.HideHealPreview();
        _animator.SetTrigger("heal_completed");
    }
}
