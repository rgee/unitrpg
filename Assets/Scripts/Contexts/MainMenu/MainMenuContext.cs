using Assets.Contexts.Base;
using Contexts.MainMenu.Commands;
using Contexts.MainMenu.Signals;
using Contexts.MainMenu.Views;
using strange.extensions.command.api;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.MainMenu {
    public class MainMenuContext : BaseContext {
        public MainMenuContext(MonoBehaviour view) : base(view) {
            
        }

        protected override void mapBindings() {
            base.mapBindings();

            mediationBinder.Bind<MainMenuView>().To<MainMenuMediator>();

            commandBinder.Bind<OptionsSignal>().To<LoadOptionsCommand>();
            commandBinder.Bind<NewGameSignal>().To<NewGameCommand>();
        }
    }
}