using Contexts.Battle;
using strange.extensions.context.impl;
using strange.extensions.mediation.impl;

namespace Assets.Contexts.Chapters.EventTesting {
    public class EventTestingRoot : BattleRoot {
        void Awake() {
            context = new EventTestingChapter(this);
        }
    }
}