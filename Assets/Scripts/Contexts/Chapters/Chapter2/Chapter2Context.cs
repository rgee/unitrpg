

using Assets.Contexts.Chapters.Chapter2.Models;
using Assets.Contexts.Chapters.Chapter2.Views;
using Contexts.Battle;
using Contexts.Battle.Signals;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.Chapters.Chapter2 {
    public class Chapter2Context : BattleContext {
        public Chapter2Context(MonoBehaviour view) : base(view) {
        }

        protected override void mapBindings() {
            base.mapBindings();

            injectionBinder.Bind<EastmerePlazaState>().ToSingleton();
            mediationBinder.Bind<Chapter2View>().To<Chapter2ViewMediator>();

            commandBinder.GetBinding<BattleStartSignal>().To<BindBattleEventsCommand>().InParallel();
        }
    }
}