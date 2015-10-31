using System.Collections;
using System.Collections.Generic;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu {
    [RequireComponent(typeof(IActionMenuView))]
    public class ActionMenu : MonoBehaviour {
        private IActionMenuView _view;

        public delegate void FightSelectedEventHandler();
        public event FightSelectedEventHandler OnFightSelected;

        public delegate void ActionSelectedEventHandler(CombatAction action);
        public event ActionSelectedEventHandler OnActionSelected;


        void Awake() {
            _view = GetComponent<IActionMenuView>();
        }

        public void Show(IEnumerable<CombatAction> actions) {
           StartCoroutine(AwaitActionSelect(actions));
        }

        public void Hide() {
            StopAllCoroutines();
            _view.SelectedAction = null;
            StartCoroutine(_view.Hide());
        }

        IEnumerator AwaitActionSelect(IEnumerable<CombatAction> actions) {
           _view.Show(actions);
            while (_view.SelectedAction == null && !_view.FightSelected) {
                yield return null;
            }

            yield return StartCoroutine(_view.Hide());

            if (_view.FightSelected) {
                if (OnFightSelected != null) {
                    OnFightSelected();
                }
            } else if (_view.SelectedAction != null) {
                if (OnActionSelected != null) {
                    OnActionSelected(_view.SelectedAction.Value);
                }
            }
            _view.SelectedAction = null;
            _view.FightSelected = false;
        }
    }
}