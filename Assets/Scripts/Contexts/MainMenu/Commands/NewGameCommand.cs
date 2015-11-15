using System.Linq;
using Assets.Contexts.Common.Services;
using Contexts.Common.Model;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using strange.extensions.command.impl;

namespace Contexts.MainMenu.Commands {
    public class NewGameCommand : Command {
        private static readonly string CutsceneSceneName = "Cutscene";

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public LoadSceneSignal LoadSceneSignal { get; set; }

        [Inject]
        public ApplicationState ApplicationState { get; set; }

        [Inject]
        public ICutsceneLoader CutsceneLoader { get; set; }

        public override void Execute() {
            SaveGameService.Reset();

            var battleConfig = BattleConfigRepository.GetConfigByIndex(0);
            var preBattleCutscenes = battleConfig.DialogueResourceSequence;
            if (preBattleCutscenes.Count > 0) {
                var parsedCutscenes = preBattleCutscenes.Select(name => CutsceneLoader.Load(name)).ToList();
                ApplicationState.CurrentCutsceneSequence = parsedCutscenes;
                LoadSceneSignal.Dispatch(CutsceneSceneName);
            } else {
                LoadSceneSignal.Dispatch(battleConfig.InitialSceneName);
            }
        }
    }
}
