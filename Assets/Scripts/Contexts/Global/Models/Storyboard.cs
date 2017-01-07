
using Assets.Contexts.Common.Services;
using C5;
using Contexts.Common.Model;
using Contexts.Global.Signals;

namespace Assets.Scripts.Contexts.Global.Models {
    public class Storyboard {
        [Inject]
        public ChangeSceneSignal ChangeSceneSignal { get; set; }

        [Inject]
        public ApplicationState State { get; set; }

        [Inject]
        public CutsceneLoader CutsceneLoader { get; set; }

        private readonly LinkedList<IStoryboardScene> _scenes;

        public Storyboard() {
            _scenes = new LinkedList<IStoryboardScene> {
               CreateCutscene("Chapter 1/Intro/male_soldier_report"),
               CreateCutscene("Chapter 1/Intro/female_soldier_report"),
               CreateCutscene("Chapter 1/Intro/liat_janek_prep"),
               CreateCutscene("Chapter 1/Intro/liad_audric_h2h"),
               CreateCutscene("Chapter 1/Intro/liat_audric_balcony"),
               CreateCutscene("Chapter 1/Intro/liat_audric_overlook"),
               new StoryboardScene("chapter_1_battle")
            };
        }

        private IStoryboardScene CreateCutscene(string name) {
            return new Cutscene(CutsceneLoader, "Cutscene", name);
        }
    }
}