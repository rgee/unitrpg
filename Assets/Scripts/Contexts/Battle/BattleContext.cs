

using Assets.Contexts.Base;
using Contexts.Base.Signals;
using Contexts.Battle.Commands;
using Contexts.Battle.Models;
using Contexts.Battle.Signals;
using Contexts.Battle.Views;
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

            ICommandBinding startBinding;
            if (this == Context.firstContext) {
                startBinding = commandBinder.GetBinding<StartSignal>();
            } else {
                startBinding = commandBinder.Bind<BattleStartSignal>();
            }

            startBinding.To<StartBattleCommand>().InSequence();

            injectionBinder.Bind<BattleViewState>().To(new BattleViewState()).ToSingleton();

            injectionBinder.Bind<BattleStartSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<HoveredTileChangeSignal>().ToSingleton();
            injectionBinder.Bind<GatherBattleFromEditorSignal>().ToSingleton();
            injectionBinder.Bind<InitializeMapSignal>().ToSingleton();
            injectionBinder.Bind<MapPositionClickedSignal>().ToSingleton();
            injectionBinder.Bind<UnitSelectedSignal>().ToSingleton();

            commandBinder.Bind<InitializeMapSignal>().To<InitializeMapCommand>();
            commandBinder.Bind<MapPositionClickedSignal>().To<SelectMapPositionCommand>();
            commandBinder.Bind<HoverPositionSignal>().To<MapHoveredCommand>();

            mediationBinder.Bind<MapView>().To<MapViewMediator>();
            mediationBinder.Bind<MapHighlightView>().To<MapHighlightViewMediator>();
        }
    }
}