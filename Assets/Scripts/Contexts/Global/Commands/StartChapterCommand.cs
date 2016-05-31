using System.Linq;
using Assets.Contexts.Common.Services;
using Contexts.Common.Model;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Global.Commands {
    public class StartChapterCommand : Command {

        private static readonly string CutsceneSceneName = "Cutscene";

        [Inject]
        public GameObject Source { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        [Inject]
        public ApplicationState ApplicationState { get; set; }

        [Inject]
        public ICutsceneLoader CutsceneLoader { get; set; }

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        public override void Execute() {
            var save = SaveGameService.CurrentSave;
            var battleConfig = BattleConfigRepository.GetConfigByIndex(save.ChapterNumber);
            var preBattleCutscenes = battleConfig.DialogueResourceSequence;
            if (preBattleCutscenes.Count > 0) {
                var parsedCutscenes = preBattleCutscenes.Select(name => CutsceneLoader.Load(name)).ToList();
                ApplicationState.CurrentCutsceneSequence = parsedCutscenes;
                ChangeSceneSignal.Dispatch(Source, CutsceneSceneName);
            } else {
                ChangeSceneSignal.Dispatch(Source, battleConfig.InitialSceneName);
            }
        }
    }
}
