using strange.extensions.context.impl;

namespace Contexts.Global {
    public class GlobalRoot : ContextView {
        void Awake() {
            context = new GlobalContext(this);
        } 
    }
}