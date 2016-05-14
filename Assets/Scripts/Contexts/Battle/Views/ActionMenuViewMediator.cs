using Assets.Contexts;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Models.Combat;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class ActionMenuViewMediator : Mediator {
        [Inject]
        public ActionMenuView View { get; set; }

        [Inject]
        public UnitSelectedSignal UnitSelectedSignal { get; set; }

        [Inject]
        public UnitDeselectedSignal UnitDeselectedSignal { get; set; }

        [Inject]
        public BackSignal BackSignal { get; set; }

        [Inject]
        public BattleViewState BattleModel { get; set; }

        [Inject]
        public CombatActionSelectedSignal ActionSelectedSignal { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            UnitSelectedSignal.AddListener(ShowActionMenu);
            UnitDeselectedSignal.AddListener(HideActionMenu);

            View.ActionTypeSelectedSignal.AddListener(HandleActionSelection);
            View.BackSignal.AddListener(RelayBackSignal);
            View.CancelSignal.AddListener(HideActionMenu);
            View.FightSignal.AddListener(ShowFightSubMenu);
        }

        private void RelayBackSignal() {
            BackSignal.Dispatch();
        }

        private void ShowFightSubMenu() {
            ActionSelectedSignal.Dispatch(CombatActionType.Fight);

            // When the user selected 'Fight', preprare to handle the Back signal
            BackSignal.AddOnce(ReturnToTop);
            View.ShowFightSubMenu();
        }

        private void ReturnToTop() {
            View.ReturnToTop();
        }


        private void HandleActionSelection(CombatActionType actionType) {
            ActionSelectedSignal.Dispatch(actionType);
            HideActionMenu();
        }

        private void ShowActionMenu(Vector3 unitPosition) {
            View.Show(unitPosition, BattleModel.AvailableActions);
        }

        private void HideActionMenu() {
            View.Hide();
            StrangeUtils.RemoveOnceListener(BackSignal, ReturnToTop);
        }
    }
}