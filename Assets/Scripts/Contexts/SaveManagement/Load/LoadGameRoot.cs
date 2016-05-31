using strange.extensions.context.impl;

namespace Assets.Contexts.SaveManagement.Load {
    public class LoadGameRoot : ContextView {
        void Awake() {
            context = new LoadGameContext(this);
        } 
    }
}