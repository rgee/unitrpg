using System;
using Assets.Scripts.Contexts.Global.Models;
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
        public Storyboard Storyboard { get; set; }

        [Inject]
        public NextStoryboardSceneSignal NextStoryboardSceneSignal { get; set; }

        [Inject(ContextKeys.CONTEXT_VIEW)]
        public GameObject Context { get; set; }

        public override void Execute() {
            SaveGameService.Choose(SelectedSave);

            var lastIndex = Storyboard.FindIndexForId(SelectedSave.LastSceneId);
            if (lastIndex < 0) {
                throw new Exception("Could not find storyboard scene by id " + SelectedSave.LastSceneId);
            }
            Storyboard.StoryboardIndex = lastIndex;
            NextStoryboardSceneSignal.Dispatch(Context);
        }
    }
}
