using Contexts.Common.Model;
using Contexts.Global.Signals;
using strange.extensions.command.impl;

namespace Contexts.MainMenu.Commands {
    public class NewGameCommand : Command {
        [Inject]
        public ISaveGameRepository SaveGameRepository { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public LoadSceneSignal LoadSceneSignal { get; set; }

        public override void Execute() {
            SaveGameRepository.Reset();

            var battleConfig = BattleConfigRepository.GetConfigByIndex(0);
            LoadSceneSignal.Dispatch(battleConfig.InitialSceneName);
        }
    }
}
