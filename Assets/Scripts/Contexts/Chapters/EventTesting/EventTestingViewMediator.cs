using strange.extensions.mediation.impl;

namespace Assets.Contexts.Chapters.EventTesting {
    public class EventTestingViewMediator : Mediator {
         [Inject]
         public DummyEventLoggerService EventLogger { get; set; }
    }
}