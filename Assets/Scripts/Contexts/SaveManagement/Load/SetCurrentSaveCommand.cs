using Contexts.Global.Services;
using Contexts.Global.Signals;
using Models.SaveGames;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Assets.Contexts.SaveManagement.Load {
    public class SetCurrentSaveCommand : Command {

        [Inject]
        public ISaveGameService SaveGameService { get; set; }

        [Inject]
        public ISaveGame SelectedSave { get; set; }

        [Inject]
        public StartChapterSignal StartChapterSignal { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject Context { get; set; }

        public override void Execute() {
            SaveGameService.Choose(SelectedSave);
            StartChapterSignal.Dispatch(Context);
        }
    }
}
