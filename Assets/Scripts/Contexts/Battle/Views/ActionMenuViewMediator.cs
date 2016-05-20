using System;
using Assets.Contexts;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
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

            View.ItemSelectedSignal.AddListener(OnItemSelect);
            View.DismissSignal.AddListener(RelayBackSignal);

        }

        private void OnItemSelect(string item) {
            var result = (CombatActionType)Enum.Parse(typeof(CombatActionType), item);
            ActionSelectedSignal.Dispatch(result);
        }

        private void RelayBackSignal() {
            View.Hide();
            BackSignal.Dispatch();
        }

        private void ShowActionMenu(Vector3 unitPosition) {
            View.transform.localPosition = unitPosition;

            var config = BubbleMenuUtils.CreateFromActions(BattleModel.AvailableActions);
            View.Show(config);
        }

        private void HideActionMenu() {
            View.Hide();
        }
    }
}