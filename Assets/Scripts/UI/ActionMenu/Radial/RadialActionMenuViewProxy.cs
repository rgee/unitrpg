using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu.Radial {
    public class RadialActionMenuViewProxy : MonoBehaviour, IActionMenuView {
        public CombatAction? SelectedAction { get; set; }


        public bool FightSelected { get; set; }

        [Tooltip("For when there are enemies nearby, but no friendlies")]
        public GameObject MoveFightFull;

        [Tooltip("For when there are no enemies or friendly units around and the unit can still move")]
        public GameObject MoveBraceItemWait;

        [Tooltip("For when there are no enemies nearby, nor friendlies and the unit cannot move")]
        public GameObject BraceItem;

        [Tooltip("For when you've used an action in place, but can still move.")]
        public GameObject MoveWait;

        [Tooltip("For when you've moved as far as you can, but can still act")]
        public GameObject FightWaitItem;

        [Tooltip("For when you've chosen to fight and all you can do is attack or brace.")]
        public GameObject AttackBrace;


        private GameObject _openMenu;
        private readonly Dictionary<CombatAction, GameObject> _prefabsByActions = new Dictionary<CombatAction, GameObject>();

        public void Awake() {
            _prefabsByActions.Add(
                CombatAction.Fight | CombatAction.Wait | CombatAction.Move | CombatAction.Item,
                MoveFightFull
            );

            _prefabsByActions.Add(
                CombatAction.Move | CombatAction.Wait | CombatAction.Brace | CombatAction.Item,
                MoveBraceItemWait
            );

            _prefabsByActions.Add(
                CombatAction.Fight | CombatAction.Wait | CombatAction.Item,
                FightWaitItem
            );

            _prefabsByActions.Add(
                CombatAction.Wait | CombatAction.Brace | CombatAction.Item,
                BraceItem
            );

            _prefabsByActions.Add(
                CombatAction.Wait | CombatAction.Move,
                MoveWait
            );

            _prefabsByActions.Add(
                CombatAction.Attack | CombatAction.Brace,
                AttackBrace
            );
        }

        public void Show(IEnumerable<CombatAction> actions) {
            var availableActions = actions 
                .Aggregate((value, next) => value | next);
            var menuPrefab = MoveBraceItemWait;
            if (_prefabsByActions.ContainsKey(availableActions)) {
                menuPrefab = _prefabsByActions[availableActions];
            } else {
                Debug.LogWarning("Could not match menu item.");
            }

            var menu = Instantiate(menuPrefab);
            menu.transform.SetParent(transform);
            menu.transform.position = Vector3.zero;
            menu.transform.localPosition = Vector3.zero;

            _openMenu = menu;
            var actionSelector = menu.GetComponent<NamedActionSelector>();
            StartCoroutine(AwaitAction(actionSelector));
        }

        private IEnumerator AwaitAction(NamedActionSelector actionSelector) {
            while (actionSelector.SelectedAction == null) {
                yield return null;
            }

            if (actionSelector.SelectedAction == CombatAction.Fight) {
                FightSelected = true;
            } else {
                SelectedAction = actionSelector.SelectedAction;
            }
        } 

        public IEnumerator Hide() {
            Destroy(_openMenu);
            _openMenu = null;
            yield return null;
        }

        private NamedActionSelector ShowFightSubMenu() {
            var state = CombatObjects.GetBattleState();
            var battle = state.Model;
            var previouslyTargetedUnit = state.SelectedUnit;
            Hide();

            var availableCombatActions = battle.GetAvailableFightActions(previouslyTargetedUnit.model)
                                               .Aggregate((value, next) => value | next);

            if (_prefabsByActions.ContainsKey(availableCombatActions)) {
                var menuPrefab = _prefabsByActions[availableCombatActions];
                var menu = Instantiate(menuPrefab);
                menu.transform.SetParent(transform);
                menu.transform.position = Vector3.zero;
                menu.transform.localPosition = Vector3.zero;

                _openMenu = menu;

                return menu.GetComponent<NamedActionSelector>();
            } else {
                throw new ArgumentException("Could not find fight menu for actions.");
            }
        }
        
    }
}