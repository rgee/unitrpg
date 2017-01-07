﻿using Contexts.BattlePrep.Signals;
using Contexts.Common.Model;
using Contexts.Global.Services;
using strange.extensions.command.impl;

namespace Contexts.BattlePrep.Commands {
    public class UpdateObjectiveCommand : Command {
        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public NewBattleConfigSignal BattleConfigSignal { get; set; }

        public override void Execute() {
            var saveGame = SaveGameService.CurrentSave;
            var config = BattleConfigRepository.GetConfigByIndex(saveGame.LastCompletedChapterNumber);

            BattleConfigSignal.Dispatch(config);
        }
    }
}
