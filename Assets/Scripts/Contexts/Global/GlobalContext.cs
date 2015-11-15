

using Assets.Contexts.Base;
using Contexts.Common.Model;
using Contexts.Global.Commands;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Global {
    public class GlobalContext : BaseContext {
        public GlobalContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            injectionBinder.Bind<RevealScreenSignal>().ToSingleton();
            injectionBinder.Bind<FadeScreenSignal>().ToSingleton();
            injectionBinder.Bind<ScreenRevealedSignal>().ToSingleton();
            injectionBinder.Bind<ScreenFadedSignal>().ToSingleton();

            injectionBinder.Bind<IBattleConfigRepository>().To<BattleConfigRepository>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISaveGameService>().To<SaveGameService>().ToSingleton().CrossContext();
            injectionBinder.Bind<LoadSceneSignal>().ToSingleton().CrossContext();

            commandBinder.Bind<LoadSceneSignal>()
                .To<FadeSceneBlackCommand>()
                .To<LoadSceneCommand>()
                .To<RevealSceneCommand>()
                .InSequence();

            mediationBinder.Bind<GlobalView>().To<GlobalViewMediator>();
        }
    }
}