using System;
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
                    var localCharacter = character;
                    var unitsForCharacter = from unit in unitModels
                                            where unit.Character.Name == localCharacter.Name
                                            select unit;

                    foreach (var unit in unitsForCharacter) {
                        var copy = new Character(character);
                        unit.Character = copy;
                    }
                }
            }

            var interactiveTiles = new List<InteractiveTile>();

            // Find all the interactive tiles in the game world and convert them to the model
            foreach (var tile in FindObjectsOfType<global::Combat.Interactive.InteractiveTile>()) {
                var tileModel = new InteractiveTile(tile.Id, tile.GridPosition, tile.Repeatable, CreateTilePredicate(tile));

                interactiveTiles.Add(tileModel);
            }

            var map = new Map(unitModels, interactiveTiles);
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

        private static Func<bool> CreateTilePredicate(global::Combat.Interactive.InteractiveTile tile) {
            return tile.CanBeUsed;
        }

        void CompleteInitialization() {
            _animator.SetTrigger("initialization_complete");
        }
    }
}