

using Assets.Contexts.Base;
using Contexts.Base.Signals;
using UnityEngine;

namespace Assets.Contexts.Main {
    public class MainContext : BaseContext {
        public MainContext(MonoBehaviour view) : base(view) {

        }

        protected override void mapBindings() {
            base.mapBindings();
            commandBinder.GetBinding<StartSignal>()
                .To<StartApplicationCommand>()
                .InSequence();
        }
    }
}