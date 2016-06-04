using strange.extensions.context.impl;

namespace Assets.Contexts.FullscreeenDialogue {
    public class FullscreenDialogueRoot : ContextView {
        void Awake() {
            context = new FullscreenDialogueContext(this);
        }       
    }
}