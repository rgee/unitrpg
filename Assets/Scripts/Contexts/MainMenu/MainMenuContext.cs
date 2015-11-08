using Assets.Contexts.Base;
using Contexts.MainMenu.Commands;
using Contexts.MainMenu.Signals;
using Contexts.MainMenu.Views;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using strange.extensions.context.impl;
using UnityEngine;

namespace Contexts.MainMenu {
    public class MainMenuContext : BaseContext {
        public MainMenuContext(MonoBehaviour view) : base(view) {
            
        }

        protected override void addCoreComponents() {
            base.addCoreComponents();
            injectionBinder.Unbind<ICommandBinder>();
            injectionBinder.Bind<ICommandBinder>().To<SignalCommandBinder>().ToSingleton();
        }

        protected override void mapBindings() {
            base.mapBindings();
            mediationBinder.Bind<MainMenuView>().To<MainMenuMediator>();
            commandBinder.Bind<OptionsSignal>().To<LoadOptionsCommand>();
        }
    }
}