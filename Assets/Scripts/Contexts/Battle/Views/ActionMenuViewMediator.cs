using Contexts.Battle.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class ActionMenuViewMediator : Mediator {
        [Inject]
        public ActionMenuView View { get; set; }

        [Inject]
        public UnitSelectedSignal UnitSelectedSignal { get; set; }

        public override void OnRegister() {
            base.OnRegister();

            UnitSelectedSignal.AddListener(View.Show);
        }

        private void HideActionMenu() {
            View.Hide();
        }
    }
}