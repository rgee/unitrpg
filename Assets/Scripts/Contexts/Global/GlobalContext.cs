

using Assets.Contexts.Base;
using Assets.Contexts.Common.Services;
using Contexts.Common.Model;
using Contexts.Global.Commands;
using Contexts.Global.Models;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using Models.Fighting.Characters;
using Models.SaveGames;
using Newtonsoft.Json.Linq;
using strange.extensions.context.api;
using strange.extensions.injector.api;
using UnityEngine;

namespace Contexts.Global {
    public sealed class GlobalContext : BaseContext {


        private class SingletonBinder<P> {
            private readonly IInjectionBinding _binding;

            public SingletonBinder(IInjectionBinder binder) {
                _binding = binder.Bind<P>();
            }

            public void ByWayOf<T>() where T : P {
                _binding.To<T>().ToSingleton().CrossContext();
            }
        }

        public GlobalContext(MonoBehaviour view) : base(view) { }

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
            ConcreteSingleton<StartChapterSignal>();
            ConcreteSingleton<ChangeSceneMultiSignal>();
            ConcreteSingleton<PushSceneSignal>();
            ConcreteSingleton<PopSceneSignal>();
            ConcreteSingleton<ScenePopCompleteSignal>();

            //injectionBinder.Bind<Game>().ToValue(wholeGame).CrossContext();

            Singleton<ICutsceneLoader>().ByWayOf<CutsceneLoader>();

            commandBinder.Bind<StartChapterSignal>()
                .To<StartChapterCommand>();

            commandBinder.Bind<PushSceneSignal>()
                .To<FadeSceneBlackCommand>()
                .To<PushSceneCommand>()
                .To<RevealSceneCommand>()
                .InSequence();

            commandBinder.Bind<PopSceneSignal>()
                .To<FadeSceneBlackCommand>()
                .To<PopSeneCommand>()
                .To<RevealSceneCommand>()
                .To<CompleteScenePopCommand>()
                .InSequence();

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