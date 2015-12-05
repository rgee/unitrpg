using strange.extensions.context.impl;

namespace Contexts.MainMenu {
    public class MainMenuRoot : ContextView {
        void Awake() {
            context = new MainMenuContext(this);
        } 
    }
}