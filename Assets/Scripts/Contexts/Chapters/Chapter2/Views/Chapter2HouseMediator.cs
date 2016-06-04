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
        public HouseLightEnableSignal HouseLightEnableSignal { get; set; }

        [Inject]
        public HouseLightTransitionCompleteSignal LightTransitionCompleteSignal { get; set; }

        public override void OnRegister() {
            HouseLightEnableSignal.AddListener(OnHouseEnable);
            HouseLightDisableSignal.AddListener(OnHouseDisable);
            View.LightTransitionCompleteSignal.AddListener(LightTransitionCompleteSignal.Dispatch);
        }

        private void OnHouseDisable(House house) {
            if (View.HouseType == house) {
                View.StartDisabling();
            }
        }

        private void OnHouseEnable(House house) {
            if (View.HouseType == house) {
                View.StartEnabling();
            }
        }
    }
}