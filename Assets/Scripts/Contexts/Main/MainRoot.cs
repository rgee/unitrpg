using strange.extensions.context.impl;

namespace Assets.Contexts.Main {
    public class MainRoot : ContextView {
        void Awake() {
            context = new MainContext(this);
        } 
    }
}