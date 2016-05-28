

using Assets.Contexts.Base;
using Contexts.Battle;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.Chapters.EventTesting {
    public class EventTestingChapter : BattleContext {
        public EventTestingChapter(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();

            injectionBinder.Bind<DummyEventLoggerService>().ToSingleton();

            mediationBinder.Bind<EventTestingView>().To<EventTestingViewMediator>();
        }
    }
}