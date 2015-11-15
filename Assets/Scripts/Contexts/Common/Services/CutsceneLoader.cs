using System;
using System.IO;
using Models.Dialogue;
using UnityEngine;

namespace Assets.Contexts.Common.Services {
    public class CutsceneLoader : ICutsceneLoader {
        private static readonly string CutscenePathBase = "Dialogue";

        public Cutscene Load(string resourcePath) {
            var path = CutscenePathBase + "/" + resourcePath;
            var cutsceneAsset = Resources.Load(path) as TextAsset;

            if (cutsceneAsset != null) {
                return DialogueUtils.ParseFromJson(cutsceneAsset.text);
            } 

            throw new ArgumentException("Cutscene not found at " + path);
        }
    }
}