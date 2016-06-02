using strange.extensions.context.impl;

namespace Assets.Contexts.OverlayDialogue {
    public class OverlayDialogueRoot : ContextView {
        void Awake() {
            context = new OverlayDialogueContext(this);
        } 
    }
}