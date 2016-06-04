using System.Runtime.InteropServices;
using Assets.Contexts.Chapters.Chapter2.Models;
using DG.Tweening;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Assets.Contexts.Chapters.Chapter2.Views {
    public class Chapter2HouseView : View {
        public House HouseType;
        public Signal LightTransitionCompleteSignal = new Signal();

        public bool Enabled;

        public void StartDisabling() {
            if (!Enabled) {
                return;
            }

            var lights = GetComponentsInChildren<HouseWindowLight>();
            var sequence = DOTween.Sequence();
            foreach (var houseLight in lights) {
                sequence.Insert(0, houseLight.GetTurnOutTween());
            }

            sequence.OnComplete(() => {
                LightTransitionCompleteSignal.Dispatch();
                Enabled = false;
            });

            sequence.Play();
        }

        public void StartEnabling() {
            if (Enabled) {
                return;
            } 

            var lights = GetComponentsInChildren<HouseWindowLight>();
            var sequence = DOTween.Sequence();
            foreach (var houseLight in lights) {
                sequence.Insert(0, houseLight.GetTurnOnTween());
            }

            sequence.OnComplete(() => {
                LightTransitionCompleteSignal.Dispatch();
                Enabled = true;
            });

            sequence.Play();
        }
    }
}