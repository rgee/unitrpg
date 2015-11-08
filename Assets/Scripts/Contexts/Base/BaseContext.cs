

using Assets.Contexts.Application.Commands;
using Assets.Contexts.Application.Signals;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using UnityEngine;

namespace Assets.Contexts.Base {
    public class BaseContext : MVCSContext {
        public BaseContext(MonoBehaviour view) : base(view) {

        }

        protected override void addCoreComponents() {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        protected override void mapBindings() {
            injectionBinder.Bind<QuitGameSignal>().ToSingleton().CrossContext();
            commandBinder.Bind<QuitGameSignal>().To<QuitGameCommand>();
        }
    }
}