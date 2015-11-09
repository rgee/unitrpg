using Contexts.BattlePrep.Signals;
using Contexts.Common.Model;
using strange.extensions.command.impl;

namespace Contexts.BattlePrep.Commands {
    public class UpdateObjectiveCommand : Command {
        [Inject]
        public ISaveGameRepository SaveGameRepository { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public NewBattleConfigSignal BattleConfigSignal { get; set; }

        public override void Execute() {
            var saveGame = SaveGameRepository.CurrentGame;
            var config = BattleConfigRepository.GetConfigByIndex(saveGame.Chapter);

            BattleConfigSignal.Dispatch(config);
        }
    }
}
