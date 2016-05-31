using Assets.Contexts.Application.Signals;
using Contexts.Base.Commands;
using Contexts.Base.Signals;
using Contexts.Common.Model;
using Contexts.Common.Utils;
using Contexts.Global.Services;
using Models.Fighting.Characters;
using Models.SaveGames;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.Base {
    public class BaseContext : MVCSContext {
        public BaseContext(MonoBehaviour view) : base(view) {

        }

        public override void Launch() {
            base.Launch();
            if (this == Context.firstContext) {
                var startSignal = (StartSignal) injectionBinder.GetInstance<StartSignal>();
                startSignal.Dispatch();
            }
        }


        protected override void addCoreComponents() {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        protected override void mapBindings() {
            var startBinding = commandBinder.Bind<StartSignal>();
            if (this == Context.firstContext) {
                // Load the Global context before issuing the start command if it hasn't been loaded.
                startBinding.To<RootStartCommand>().To<StartCommand>().InSequence();

                injectionBinder.Bind<ApplicationState>().ToValue(new ApplicationState()).CrossContext();
                injectionBinder.Bind<IRoutineRunner>().To<RoutineRunner>().CrossContext();

                var saveRepository = new DiskSaveGameRepository(UnityEngine.Application.persistentDataPath);
                injectionBinder.Bind<ISaveGameRepository>().ToValue(saveRepository).CrossContext();

                injectionBinder.Bind<ISaveGameService>().To<SaveGameService>().ToSingleton().CrossContext();
                injectionBinder.Bind<CharacterDatabase>().To<BaseCharacterDatabase>().ToSingleton().CrossContext();
                injectionBinder.Bind<IBattleConfigRepository>().To<BattleConfigRepository>().ToSingleton().CrossContext();
                injectionBinder.Bind<AddSceneSignal>().ToSingleton().CrossContext();
                injectionBinder.Bind<QuitGameSignal>().ToSingleton().CrossContext();
                commandBinder.Bind<AddSceneSignal>().To<AddSceneCommand>();
                commandBinder.Bind<QuitGameSignal>().To<QuitGameCommand>();
            } else {
                startBinding.To<KillAudioListenerCommand>().To<StartCommand>().InSequence();
            }

        }
    }
}