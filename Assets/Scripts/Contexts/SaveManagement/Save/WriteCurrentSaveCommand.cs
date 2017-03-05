using Contexts.Common.Model;
using Contexts.Global.Services;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Assets.Contexts.SaveManagement.Save {
    public class WriteCurrentSaveCommand : Command {
        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject Context { get; set; }

        public override void Execute() {
            if (SaveGameService.CurrentSave == null) {
                Debug.LogError("Could not write to save file. No save file present.");
            }
        }
    }
}
