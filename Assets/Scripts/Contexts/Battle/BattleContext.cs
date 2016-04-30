

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
            injectionBinder.Bind<InitializeMapSignal>().ToSingleton();
            injectionBinder.Bind<MapPositionClickedSignal>().ToSingleton();
            injectionBinder.Bind<UnitSelectedSignal>().ToSingleton();

            commandBinder.Bind<BattleStartSignal>()
                .To<StartBattleCommand>();
            commandBinder.Bind<InitializeMapSignal>().To<InitializeMapCommand>();
            commandBinder.Bind<MapPositionClickedSignal>().To<SelectMapPositionCommand>();
        }
    }
}