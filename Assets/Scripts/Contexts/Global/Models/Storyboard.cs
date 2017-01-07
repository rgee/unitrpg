﻿
using Assets.Contexts.Common.Services;
using C5;
using Contexts.Common.Model;
using Contexts.Global.Signals;

namespace Assets.Scripts.Contexts.Global.Models {
    public class Storyboard {
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        public ApplicationState State { get; set; }

        public ICutsceneLoader CutsceneLoader { get; set; }

        private readonly LinkedList<IStoryboardScene> _scenes;

        private int _storyboardIndex = 0;

        [Construct]
        public Storyboard(ChangeSceneSignal changeSceneSignal, ApplicationState state, ICutsceneLoader cutsceneLoader) {
            ChangeSceneSignal = changeSceneSignal;
            State = state;
            CutsceneLoader = cutsceneLoader;

            _scenes = new LinkedList<IStoryboardScene> {
               CreateCutscene("Chapter 1/Intro/male_soldier_report"),
               CreateCutscene("Chapter 1/Intro/female_soldier_report"),
               CreateCutscene("Chapter 1/Intro/liat_janek_prep"),
               CreateCutscene("Chapter 1/Intro/liat_audric_h2h"),
               CreateCutscene("Chapter 1/Intro/liat_audric_balcony"),
               CreateCutscene("Chapter 1/Intro/liat_audric_overlook"),
               new StoryboardScene("chapter_1_battle")
            };
        }

        public IStoryboardScene GetAndIncrementNextScene() {
            var result = _scenes[_storyboardIndex];
            _storyboardIndex++;
            return result;
        }

        private IStoryboardScene CreateCutscene(string name) {
            return new Cutscene(CutsceneLoader, "Cutscene", name);
        }
    }
}