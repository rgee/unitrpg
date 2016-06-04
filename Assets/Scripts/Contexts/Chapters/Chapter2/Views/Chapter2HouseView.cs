using System.Runtime.InteropServices;
using Assets.Contexts.Chapters.Chapter2.Models;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Assets.Contexts.Chapters.Chapter2.Views {
    public class Chapter2HouseView : View {
        public House HouseType;
        public Signal DisablingCompleteSignal = new Signal();

        private bool _enabled;

        public void StartDisabling() {
            if (!_enabled) {
                return;
            }
            DisablingCompleteSignal.Dispatch();
            _enabled = false;
        }

        public void StartEnabling() {
            if (_enabled) {
                return;
            } 
            _enabled = true;
        }
    }
}