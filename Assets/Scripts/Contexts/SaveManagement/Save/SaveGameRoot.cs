using strange.extensions.context.impl;

namespace Assets.Contexts.SaveManagement.Save {
    public class SaveGameRoot : ContextView {
        void Awake() {
            context = new SaveGameContext(this);
        } 
    }
}