

using Assets.Contexts.Base;
using Contexts.Battle.Commands;
using Contexts.Battle.Signals;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Battle {
    public class BattleContext : BaseContext {
        public BattleContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();
            injectionBinder.Bind<BattleStartSignal>().ToSingleton().CrossContext();
            commandBinder.Bind<BattleStartSignal>()
                .To<StartBattleCommand>();
        }
    }
}