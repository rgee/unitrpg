
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Contexts.MainMenu.Views {
    public class MainMenuView : View {
        public Signal NewGameClicked = new Signal();
        public Signal LoadGameClicked = new Signal();
        public Signal OptionsClicked = new Signal();
        public Signal QuitClicked = new Signal();

        public void Quit() {
            QuitClicked.Dispatch();
        }

        public void Options() {
            OptionsClicked.Dispatch();
        }

        public void NewGame() {
            NewGameClicked.Dispatch();
        }
    }
}