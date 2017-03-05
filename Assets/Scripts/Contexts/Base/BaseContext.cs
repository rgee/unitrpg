using System;
using Assets.Contexts.Application.Signals;
using Assets.Contexts.Common.Services;
using Assets.Scripts.Contexts.Global.Models;
using Contexts.Base.Commands;
using Contexts.Base.Signals;
using Contexts.Common.Model;
using Contexts.Common.Utils;
using Contexts.Global.Models;
using Contexts.Global.Services;
using Models.Fighting.Characters;
using Models.Fighting.Maps.Configuration;
using Models.SaveGames;
using Newtonsoft.Json.Linq;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.Base {
    public class BaseContext : MVCSContext {
        private const int MaxSaves = 6;
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

                var gameConfigText = Resources.Load("Configuration/game_configuration") as TextAsset;
                var stopwatch = System.Diagnostics.Stopwatch.StartNew();
                if (gameConfigText != null) {
                    var gameJson = JObject.Parse(gameConfigText.text);
                    var game = Game.CreateFromJson(gameJson);
                    stopwatch.Stop();

                    Debug.LogFormat("Deserializing Game object took {0}ms", stopwatch.ElapsedMilliseconds);
                    injectionBinder.Bind<Game>().ToValue(game).CrossContext();
                    injectionBinder.Bind<IMapConfigRepository>().To(new ExternalMapConfigurationRepository(game.Maps));
                } else {
                    throw new Exception("Could not find game configuration file.");
                }

                injectionBinder.Bind<ICutsceneLoader>().To<CutsceneLoader>().ToSingleton().CrossContext();
                injectionBinder.Bind<Storyboard>().ToSingleton().CrossContext();
                injectionBinder.Bind<ApplicationState>().ToValue(new ApplicationState()).CrossContext();
                injectionBinder.Bind<IRoutineRunner>().To<RoutineRunner>().CrossContext();

                var saveRepository = new DiskSaveGameRepository(UnityEngine.Application.persistentDataPath, MaxSaves);
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