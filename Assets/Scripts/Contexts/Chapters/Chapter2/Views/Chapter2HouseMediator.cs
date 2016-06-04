using Assets.Contexts.Chapters.Chapter2.Models;
using Assets.Contexts.Chapters.Chapter2.Signals;
using strange.extensions.mediation.impl;

namespace Assets.Contexts.Chapters.Chapter2.Views {
    public class Chapter2HouseMediator : Mediator {
        [Inject]
        public Chapter2HouseView View { get; set; }

        [Inject]
        public HouseLightDisableSignal HouseLightDisableSignal { get; set; }

        [Inject]
        public HouseLightTransitionCompleteSignal LightTransitionCompleteSignal { get; set; }

        public override void OnRegister() {
            HouseLightDisableSignal.AddListener(OnDisable);
            View.DisablingCompleteSignal.AddListener(LightTransitionCompleteSignal.Dispatch);
        }

        private void OnDisable(House house) {
            if (View.HouseType == house) {
                View.StartDisabling();
            }
        }
    }
}