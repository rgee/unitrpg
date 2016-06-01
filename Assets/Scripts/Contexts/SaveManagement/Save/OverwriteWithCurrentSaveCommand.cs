using Contexts.Global.Services;
using Models.SaveGames;
using strange.extensions.command.impl;

namespace Assets.Contexts.SaveManagement.Save {
    public class OverwriteWithCurrentSaveCommand : Command {
        [Inject]
        public ISaveGame SelectedSaveGame { get; set; }

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        public override void Execute() {
            SaveGameService.Overwrite(SelectedSaveGame, SaveGameService.CurrentSave);
        }
    }
}
