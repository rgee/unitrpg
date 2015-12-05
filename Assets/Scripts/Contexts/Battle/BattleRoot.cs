using strange.extensions.context.impl;

namespace Contexts.Battle {
    public class BattleRoot : ContextView {
        void Awake() {
            context = new BattleContext(this);
        }
    }
}