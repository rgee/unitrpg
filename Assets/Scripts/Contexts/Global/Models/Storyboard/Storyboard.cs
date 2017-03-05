
using System;
using Assets.Contexts.Common.Services;
using C5;
using Contexts.Common.Model;
using Contexts.Global.Signals;
using Models.Fighting.Battle.Objectives;

namespace Assets.Scripts.Contexts.Global.Models {
    public class Storyboard {
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        public ApplicationState State { get; set; }


        private readonly LinkedList<IStoryboardScene> _scenes;

        private int _storyboardIndex = 0;

        public int StoryboardIndex {
            get { return _storyboardIndex; }
            set {
                if (value >= _scenes.Count) {
                    throw new ArgumentException("Cannot skip to scene index " + value + ". Too high.");
                }

                _storyboardIndex = value;
            }
        }

        public IStoryboardScene CurrentScene {
            get { return _scenes[Math.Max(0, _storyboardIndex - 1)]; }
        }

        [Construct]
        public Storyboard(ChangeSceneSignal changeSceneSignal, ApplicationState state) {
            ChangeSceneSignal = changeSceneSignal;
            State = state;

            _scenes = new LinkedList<IStoryboardScene> {
               new StoryboardScene("MaleSoldierReport", "male_soldier_report"),
               new StoryboardScene("FemaleSoldierReport", "female_soldier_report"),
               new StoryboardScene("LiatJanekPrep", "liat_janek_prep"),
               new StoryboardScene("LiatAudricH2H", "liat_audric_h2h"),
               new StoryboardScene("LiatAudricBalcony", "liat_audric_balcony"),
               new StoryboardScene("LiatAudricOverlook", "liat_audric_overlook"),
               new BattleStoryboardScene("Chapter1", "chapter_1_battle", new Rout()),
               new BattleStoryboardScene("Chapter2", "chapter_2_battle", new Rout())
            };
        }

        public int FindIndexForId(string id) {
            return _scenes.FindIndex(scene => scene.Id == id);
        }

        public IStoryboardScene GetAndIncrementNextScene() {
            var result = _scenes[_storyboardIndex];
            _storyboardIndex++;
            return result;
        }
    }
}