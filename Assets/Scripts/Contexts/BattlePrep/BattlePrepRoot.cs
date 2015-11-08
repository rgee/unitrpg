using strange.extensions.context.impl;

namespace Contexts.BattlePrep {
    public class BattlePrepRoot : ContextView {
        void Awake() {
            context = new BattlePrepContext(this);
        } 
    }
}