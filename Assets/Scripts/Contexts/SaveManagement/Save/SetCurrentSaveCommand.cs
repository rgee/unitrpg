using Contexts.Global.Services;
using Models.SaveGames;
using strange.extensions.command.impl;
using UnityEngine;

namespace Assets.Contexts.SaveManagement.Save {
    public class SetCurrentSaveCommand : Command {

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public ISaveGame SelectedSave { get; set; }

        public override void Execute() {
            SaveGameService.Choose(SelectedSave);
            Debug.LogFormat("Save game id: {0} selected", SelectedSave.Id);
        }
    }
}
