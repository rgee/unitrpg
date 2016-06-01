﻿

using Assets.Contexts.Base;
using Contexts.Base.Signals;
using Contexts.BattlePrep.Commands;
using Contexts.BattlePrep.Signals;
using Contexts.BattlePrep.Views;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using Models.SaveGames;
using strange.extensions.command.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.BattlePrep {
    public class BattlePrepContext : BaseContext {
        public BattlePrepContext(MonoBehaviour view) : base(view) {

        }

        public override void Launch() {
            base.Launch();
            if (this == Context.firstContext) {
                var startSignal = injectionBinder.GetInstance<ShowBattlePrepSignal>();
                startSignal.Dispatch();
            }
        }

        protected override void mapBindings() {
            base.mapBindings();

            if (this == Context.firstContext) {
                injectionBinder.Unbind<ISaveGameRepository>();
                injectionBinder.Bind<ISaveGameRepository>().ToValue(new TestingSaveGameRepository());
            }

            injectionBinder.Bind<ShowBattlePrepSignal>().ToSingleton().CrossContext();
            commandBinder.Bind<ShowBattlePrepSignal>()
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

            commandBinder.Bind<HideBattlePrepSignal>()
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