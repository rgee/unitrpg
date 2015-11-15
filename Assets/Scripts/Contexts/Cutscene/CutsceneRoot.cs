using strange.extensions.context.impl;

namespace Contexts.Cutscene {
    public class CutsceneRoot : ContextView {
        void Awake() {
            context = new CutsceneContext(this);
        } 
    }
}