using Contexts.MainMenu.Models;
using strange.extensions.mediation.impl;

namespace Contexts.MainMenu.Views {
    public class MainMenuBackgroundMediator : Mediator {
        [Inject]
        public CurrentTimeOfDay CurrentTime { get; set; }

        [Inject]
        public MainMenuBackgroundView View { get; set; }

        public override void OnRegister() {
            View.SetTime(CurrentTime.Time);            
        }
    }
}