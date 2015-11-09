

using Contexts.Battle.Signals;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Battle {
    public class BattleContext : MVCSContext {
        public BattleContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            injectionBinder.Bind<BattleStartSignal>().ToSingleton().CrossContext();
        }
    }
}