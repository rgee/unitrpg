using Contexts.Common.Model;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using strange.extensions.command.impl;

namespace Contexts.MainMenu.Commands {
    public class NewGameCommand : Command {
        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public IBattleConfigRepository BattleConfigRepository { get; set; }

        [Inject]
        public LoadSceneSignal LoadSceneSignal { get; set; }

        public override void Execute() {
            SaveGameService.Reset();

            var battleConfig = BattleConfigRepository.GetConfigByIndex(0);
            LoadSceneSignal.Dispatch(battleConfig.InitialSceneName);
        }
    }
}
