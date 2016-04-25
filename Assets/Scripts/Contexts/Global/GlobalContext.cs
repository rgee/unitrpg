

using Assets.Contexts.Base;
using Assets.Contexts.Common.Services;
using Contexts.Common.Model;
using Contexts.Global.Commands;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using Models.Fighting.Characters;
using Models.SaveGames;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.Global {
    public class GlobalContext : BaseContext {
        public GlobalContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            injectionBinder.Bind<RevealScreenSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<FadeScreenSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ScreenRevealedSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ScreenFadedSignal>().ToSingleton();

            injectionBinder.Bind<IBattleConfigRepository>().To<BattleConfigRepository>().ToSingleton().CrossContext();
            injectionBinder.Bind<ICutsceneLoader>().To<CutsceneLoader>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISaveGameRepository>().To<TestingSaveGameRepository>().ToSingleton().CrossContext();
            injectionBinder.Bind<ISaveGameService>().To<SaveGameService>().ToSingleton().CrossContext();
            injectionBinder.Bind<CharacterDatabase>().To<BaseCharacterDatabase>().ToSingleton().CrossContext();
            injectionBinder.Bind<LoadSceneSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ChangeSceneSignal>().ToSingleton().CrossContext();
            injectionBinder.Bind<ChangeSceneMultiSignal>().ToSingleton().CrossContext();

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