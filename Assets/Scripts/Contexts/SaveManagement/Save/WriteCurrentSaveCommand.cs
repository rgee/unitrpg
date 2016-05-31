using Contexts.Global.Services;
using strange.extensions.command.impl;
using UnityEngine;

namespace Assets.Contexts.SaveManagement.Save {
    public class WriteCurrentSaveCommand : Command {
        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        public override void Execute() {
            if (SaveGameService.CurrentSave == null) {
                Debug.LogError("Could not write to save file. No save file present.");
            }
        }
    }
}
