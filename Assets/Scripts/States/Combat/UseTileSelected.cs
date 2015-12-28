using System.Collections;
using System.Linq;
using Combat.Interactive;
using UnityEngine;

public class UseTileSelected : StateMachineBehaviour {
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        var battleState = CombatObjects.GetBattleState();
        var battle = battleState.Model;

        // Find all the available interactive objects
        var availableTiles = battle.GetAdjacentInteractiveTiles(battleState.SelectedGridPosition).ToList();

        // If there's only one , use it immediately.
        if (availableTiles.Count == 1) {
            var tile = availableTiles.First();
            var allInteractiveTiles = FindObjectsOfType<InteractiveTile>();
            foreach (var potentialTile in allInteractiveTiles) {
                // Find the component in the game world at this position
                if (tile.GridPosition == potentialTile.GridPosition) {
                    var unit = battleState.SelectedUnit;
                    potentialTile.StartCoroutine(Trigger(animator, unit, potentialTile));
                }
            }
        } else {
            // TODO: Show something to allow the player to pick which thing with which to interact.
        }
    }

    private IEnumerator Trigger(Animator animator, Grid.Unit unit, InteractiveTile tile) {
        yield return tile.StartCoroutine(tile.Use(unit));
        animator.SetTrigger("use_complete");
    }
}