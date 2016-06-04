using System.Runtime.InteropServices;
using Assets.Contexts.Chapters.Chapter2.Models;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Assets.Contexts.Chapters.Chapter2.Views {
    public class Chapter2HouseView : View {
        public House HouseType;
        public Signal DisablingCompleteSignal = new Signal();

        public bool Enabled;

        public void StartDisabling() {
            if (!Enabled) {
                return;
            }
            DisablingCompleteSignal.Dispatch();
            Enabled = false;
        }

        public void StartEnabling() {
            if (Enabled) {
                return;
            } 
            Enabled = true;
        }
    }
}