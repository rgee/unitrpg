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

        public delegate void ActionSelectedEventHandler(CombatAction action);
        public event ActionSelectedEventHandler OnActionSelected;


        void Awake() {
            _view = GetComponent<IActionMenuView>();
        }

        public void Show(IEnumerable<CombatAction> actions, IEnumerable<CombatAction> fightActions) {
           StartCoroutine(AwaitActionSelect(actions, fightActions));
        }

        public void Hide() {
            StopAllCoroutines();
            _view.Canceled = false;
            _view.SelectedAction = null;
            StartCoroutine(_view.Hide());
        }

        IEnumerator AwaitActionSelect(IEnumerable<CombatAction> actions, IEnumerable<CombatAction> fightActions) {
           _view.Show(actions, fightActions);
            while (_view.SelectedAction == null && !_view.Canceled) {
                yield return null;
            }

            if (_view.SelectedAction != null) {
                if (OnActionSelected != null) {
                    OnActionSelected(_view.SelectedAction.Value);
                }
            } else if (_view.Canceled) {
                if (OnCancel != null) {
                    OnCancel();
                }
            }

        }
    }
}