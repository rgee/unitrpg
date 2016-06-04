using System;
using System.IO;
using Assets.Contexts.OverlayDialogue.Signals;
using Models.Dialogue;
using strange.extensions.command.impl;
using UnityEngine;

namespace Assets.Contexts.OverlayDialogue.Commands {
    public class StartDialogueCommand : Command {
        [Inject]
        public string DialogueAssetPath { get; set; }

        [Inject]
        public NewCutsceneSignal NewCutsceneSignal { get; set; }

        public override void Execute() {

            var path = Path.Combine("Dialogue", DialogueAssetPath);
            var text = Resources.Load(path) as TextAsset;
            if (text == null) {
                throw new ArgumentException("Could not find dialogue at " + path);
            }

            var model = DialogueUtils.ParseFromJson(text.text);
            NewCutsceneSignal.Dispatch(model);
        }
    }
}
