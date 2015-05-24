using System;
using System.Collections.Generic;
using System.Linq;
using Models.Combat;
using UnityEngine;

public class ActionMenuManager : MonoBehaviour {
    public delegate void ActionSelectedHandler(BattleAction action);

    public event ActionSelectedHandler OnActionSelected;

    [Tooltip("For when there are enemies nearby, but no friendlies")]
    public GameObject MoveFightFull;

    [Tooltip("For when there are no enemies or friendly units around and the unit can still move")]
    public GameObject MoveBraceItem;

    [Tooltip("For when there are no enemies nearby, nor friendlies and the unit cannot move")]
    public GameObject BraceItem;

    [Tooltip("For when you've used an action in place, but can still move.")]
    public GameObject MoveWait;

    private GameObject _openMenu;
    private readonly Dictionary<CombatAction, GameObject> _prefabsByActions = new Dictionary<CombatAction, GameObject>();

    public void Start() {
        _prefabsByActions.Add(
            CombatAction.Fight | CombatAction.Wait | CombatAction.Move | CombatAction.Brace | CombatAction.Item,
            MoveFightFull
        );

        _prefabsByActions.Add(
            CombatAction.Move | CombatAction.Wait | CombatAction.Brace | CombatAction.Item,
            MoveBraceItem
        );

        _prefabsByActions.Add(
            CombatAction.Wait | CombatAction.Brace | CombatAction.Item,
            BraceItem
        );

        _prefabsByActions.Add(
            CombatAction.Wait | CombatAction.Move,
            MoveWait
        );
    }

    public void ShowActionMenu(Grid.Unit unit) {
        var battle = CombatObjects.GetBattleState().Model;

        var availableActionEnums = battle.GetAvailableActions(unit.model);

        var availableActions = availableActionEnums
            .Aggregate((value, next) => value | next);

        var menuPrefab = MoveBraceItem;
        if (_prefabsByActions.ContainsKey(availableActions)) {
            menuPrefab = _prefabsByActions[availableActions];
        } else {
            Debug.LogWarning("Could not match menu item.");
        }

        var menu = Instantiate(menuPrefab);
        menu.transform.SetParent(unit.transform, true);
        menu.transform.localPosition = new Vector3(-16, 35, 0);

        _openMenu = menu;
    }

    public void HideCurrentMenu() {
        if (_openMenu != null) {
            Destroy(_openMenu);
            _openMenu = null;
        }
    }

    public void SelectAction(string name) {
        var action = (BattleAction) Enum.Parse(typeof (BattleAction), name);
        if (OnActionSelected != null) {
            OnActionSelected(action);
        }
    }
}