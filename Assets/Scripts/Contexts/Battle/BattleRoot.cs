using strange.extensions.context.impl;

namespace Contexts.Battle {
    public class BattleRoot : ContextView {
        public string MapName;
        
        void Awake() {
            context = new BattleContext(this);
        }
    }
}