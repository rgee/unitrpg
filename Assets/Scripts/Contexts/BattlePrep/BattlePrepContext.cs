

using Assets.Contexts.Base;
using Contexts.Base.Signals;
using Contexts.BattlePrep.Commands;
using Contexts.BattlePrep.Signals;
using Contexts.BattlePrep.Views;
using Contexts.Global.Signals;
using strange.extensions.command.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.BattlePrep {
    public class BattlePrepContext : BaseContext {
        public BattlePrepContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();

            ICommandBinding startBinding;
            if (this == Context.firstContext) {
                startBinding = commandBinder.GetBinding<StartSignal>();
            } else {
                startBinding = commandBinder.Bind<ScreenRevealedSignal>();
            }

            startBinding
                .To<BattlePrepStartCommand>()
                .To<ShowPrepCommand>().InSequence();

            injectionBinder.Bind<NewBattleConfigSignal>().ToSingleton();
            injectionBinder.Bind<TransitionOutSignal>().ToSingleton();
            injectionBinder.Bind<TransitionInSignal>().ToSingleton();
            injectionBinder.Bind<TransitionCompleteSignal>().ToSingleton();
            injectionBinder.Bind<ActionSelectedSignal>().ToSingleton();

            commandBinder.Bind<ActionSelectedSignal>()
                .To<ActionSelectedCommand>();

            commandBinder.Bind<UpdateObjectiveSignal>()
                .To<UpdateObjectiveCommand>();

            commandBinder.Bind<HidePrepSignal>()
                .To<HidePrepCommand>();

            commandBinder.Bind<ClosePrepSignal>()
                .To<HidePrepCommand>();

            commandBinder.Bind<StartBattleSignal>()
                .To<HidePrepCommand>()
                .To<StartBattleCommand>()
                .InSequence();

            mediationBinder.Bind<BattlePrepView>().To<BattlePrepViewMediator>();    
        }

    }
}