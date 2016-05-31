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

        [Inject]
        public OptionsSignal OptionsSignal { get; set; }

        [Inject]
        public NewGameSignal NewGameSignal { get; set; }

        [Inject]
        public LoadGameSignal LoadGameSignal { get; set; }

        public override void OnRegister() {
            View.QuitClicked.AddListener(OnQuitClicked);
            View.OptionsClicked.AddListener(OnOptionsClicked);
            View.NewGameClicked.AddListener(OnNewGameClicked);
            View.LoadGameClicked.AddListener(LoadGameSignal.Dispatch);
        }

        private void OnNewGameClicked() {
            NewGameSignal.Dispatch();
        }

        private void OnOptionsClicked() {
            OptionsSignal.Dispatch(); 
        }

        private void OnQuitClicked() {
            QuitSignal.Dispatch();
        }
    }
}