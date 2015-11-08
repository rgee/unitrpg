using Assets.Contexts.Application.Signals;
using Contexts.MainMenu.Signals;
using strange.extensions.mediation.impl;
using UnityEngine;

namespace Contexts.MainMenu.Views {
    public class MainMenuMediator : Mediator {
        [Inject]
        public MainMenuView View { get; set; }

        [Inject]
        public QuitGameSignal QuitSignal { get; set; } 

        public override void OnRegister() {
            View.QuitClicked.AddListener(OnQuitClicked);
        }

        private void OnQuitClicked() {
            QuitSignal.Dispatch();
        }
    }
}