using System.Linq;
using Models.Combat;
using UnityEngine;

namespace States.Combat {
    public class InitializeBattle : StateMachineBehaviour {
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            var state = CombatObjects.GetBattleState();
            var unitManager = CombatObjects.GetUnitManager();

            var unitModels = from unit in unitManager.GetAllUnits()
                             select unit.model;

            var map = new Map(unitModels);
            var turnState = new Turn(map);
            var objective = new Models.Combat.Objectives.Rout();
            var actionProber = new ActionProber(map, turnState);

            state.Model = new Battle(map, objective, turnState, actionProber);
        }
    }
}