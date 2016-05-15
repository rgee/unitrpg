

using Assets.Contexts.Base;
using Assets.Contexts.Common.Services;
using Contexts.Common.Model;
using Contexts.Global.Commands;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using Models.Fighting.Characters;
using Models.SaveGames;
using strange.extensions.injector.api;
using UnityEngine;

namespace Contexts.Global {
    public class GlobalContext : BaseContext {
        private class SingletonBinder<P> {
            private readonly IInjectionBinding _binding;

            public SingletonBinder(IInjectionBinder binder) {
                _binding = binder.Bind<P>();
            }

            public void ByWayOf<T>() {
                _binding.To<T>().ToSingleton().CrossContext();
            }
        }

        public GlobalContext(MonoBehaviour view) : base(view) {

        }

        private SingletonBinder<T> Singleton<T>() {
            return new SingletonBinder<T>(injectionBinder);
        }

        private void ConcreteSingleton<T>() {
            injectionBinder.Bind<T>().ToSingleton().CrossContext();
        }

        protected override void mapBindings() {
            ConcreteSingleton<RevealScreenSignal>();
            ConcreteSingleton<FadeScreenSignal>();
            ConcreteSingleton<ScreenRevealedSignal>();
            ConcreteSingleton<ScreenFadedSignal>();
            ConcreteSingleton<LoadSceneSignal>();
            ConcreteSingleton<ChangeSceneSignal>();
            ConcreteSingleton<ChangeSceneMultiSignal>();

            Singleton<IBattleConfigRepository>().ByWayOf<BattleConfigRepository>();
            Singleton<ICutsceneLoader>().ByWayOf<CutsceneLoader>();

            commandBinder.Bind<LoadSceneSignal>()
                .To<FadeSceneBlackCommand>()
                .To<LoadSceneCommand>()
                .To<RevealSceneCommand>()
                .InSequence();

            commandBinder.Bind<ChangeSceneSignal>()
                .To<FadeSceneBlackCommand>()
                .To<ChangeSceneCommand>()
                .To<RevealSceneCommand>()
                .InSequence();

            commandBinder.Bind<ChangeSceneMultiSignal>()
                .To<FadeSceneBlackCommand>()
                .To<AddMultipleScenesCommand>()
                .To<RevealSceneCommand>()
                .InSequence();

            mediationBinder.Bind<GlobalView>().To<GlobalViewMediator>();
        }
    }
}