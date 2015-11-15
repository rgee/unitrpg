

using Assets.Contexts.Base;
using Contexts.Base.Signals;
using Contexts.BattlePrep.Commands;
using Contexts.BattlePrep.Signals;
using Contexts.BattlePrep.Views;
using UnityEngine;

namespace Contexts.BattlePrep {
    public class BattlePrepContext : BaseContext {
        public BattlePrepContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();


            var startBinding = commandBinder.GetBinding<StartSignal>();
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

            mediationBinder.Bind<BattlePrepView>().To<BattlePrepViewMediator>();    
        }

    }
}