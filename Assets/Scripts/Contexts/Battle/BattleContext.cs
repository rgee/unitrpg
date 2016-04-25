

using Assets.Contexts.Base;
using Contexts.Base.Signals;
using Contexts.Battle.Commands;
using Contexts.Battle.Signals;
using Contexts.Global.Signals;
using strange.extensions.command.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Battle {
    public class BattleContext : BaseContext {
        public BattleContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();

            if (this == Context.firstContext) {
                commandBinder.GetBinding<StartSignal>().To<StartBattleCommand>();
            }

            injectionBinder.Bind<BattleStartSignal>().ToSingleton().CrossContext();
            commandBinder.Bind<BattleStartSignal>()
                .To<StartBattleCommand>();
        }
    }
}