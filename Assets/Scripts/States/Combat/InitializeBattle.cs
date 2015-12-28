using System.Collections.Generic;
using System.Linq;
using Models;
using Models.Combat;
using SaveGames;
using UnityEngine;

namespace States.Combat {
    public class InitializeBattle : StateMachineBehaviour {
        public List<GameObject> ManagerPrefabs; 

        private Animator _animator;
        public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
            _animator = animator;
            var state = CombatObjects.GetBattleState();
            var unitManager = CombatObjects.GetUnitManager();
            var unitModels = from unit in unitManager.GetAllUnits()
                             select unit.model;

            var saveGame = BinarySaveManager.CurrentState;
            if (saveGame != null) {
                foreach (var character in saveGame.Characters) {
                    var unitsForCharacter = from unit in unitModels
                                            where unit.Character.Name == character.Name
                                            select unit;

                    foreach (var unit in unitsForCharacter) {
                        var copy = new Character(character);
                        unit.Character = copy;
                    }
                }
            }

            var map = new Map(unitModels, new List<InteractiveTile>());
            var turnState = new Turn(map);
            var objective = new Models.Combat.Objectives.Rout();
            var actionProber = new ActionProber(map, turnState);

            state.Model = new Battle(map, objective, turnState, actionProber);
            
            var directorObj = GameObject.Find("BattleIntroDirector");
            if (directorObj != null) {
                var director = directorObj.GetComponent<BattleIntroDirector>();
                director.OnIntroComplete += CompleteInitialization;
                director.StartIntro();
            } else {
                CompleteInitialization();
            }

            foreach (var prefab in ManagerPrefabs) {
                Instantiate(prefab);
            }
        }

        void CompleteInitialization() {
            _animator.SetTrigger("initialization_complete");
        }
    }
}