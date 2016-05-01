using System;
using System.Collections;
using System.Collections.Generic;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu {
    [RequireComponent(typeof(IActionMenuView))]
    public class ActionMenu : MonoBehaviour {
        private IActionMenuView _view;

        public event Action OnCancel;

        public delegate void ActionSelectedEventHandler(CombatActionType actionType);
        public event ActionSelectedEventHandler OnActionSelected;


        void Awake() {
            _view = GetComponent<IActionMenuView>();
        }

        public void Show(IEnumerable<CombatActionType> actions, IEnumerable<CombatActionType> fightActions) {
           StartCoroutine(AwaitActionSelect(actions, fightActions));
        }

        public void Hide() {
            StopAllCoroutines();
            _view.Canceled = false;
            _view.SelectedActionType = null;
            StartCoroutine(_view.Hide());
        }

        IEnumerator AwaitActionSelect(IEnumerable<CombatActionType> actions, IEnumerable<CombatActionType> fightActions) {
           _view.Show(actions, fightActions);

            // Keep yielding until the view has been dismissed or has an action.
            while (_view.SelectedActionType == null && !_view.Canceled) {
                yield return null;
            }

            if (_view.SelectedActionType != null) {
                if (OnActionSelected != null) {
                    OnActionSelected(_view.SelectedActionType.Value);
                }
            } else if (_view.Canceled) {
                if (OnCancel != null) {
                    OnCancel();
                }
            }

        }
    }
}