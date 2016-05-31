using System;
using Assets.Contexts.Base;
using Contexts.MainMenu.Commands;
using Contexts.MainMenu.Models;
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

            var time = new CurrentTimeOfDay(GetTimeOfDay());
            injectionBinder.Bind<CurrentTimeOfDay>().ToValue(time);

            mediationBinder.Bind<MainMenuView>().To<MainMenuMediator>();
            mediationBinder.Bind<MainMenuBackgroundView>().To<MainMenuBackgroundMediator>();

            commandBinder.Bind<OptionsSignal>().To<LoadOptionsCommand>();
            commandBinder.Bind<NewGameSignal>().To<NewGameCommand>();
            commandBinder.Bind<LoadGameSignal>().To<LoadGameCommand>();
        }

        private static TimeOfDay GetTimeOfDay() {
            var timeOfDay = DateTime.Now.TimeOfDay;
            var hours = timeOfDay.Hours;

            if (hours > 4 && hours <= 9) {
                return TimeOfDay.Sunrise;
            }

            if (hours > 9 && hours <= 17) {
                return TimeOfDay.Day;
            }

            if (hours > 17 && hours < 21) {
                return TimeOfDay.Sunset;
            }

            if (hours >= 21 || hours <= 4) {
                return TimeOfDay.Night;
            }

            Debug.LogErrorFormat("Couldn't figure out which time of day to show. Hours={0}", hours);
            return TimeOfDay.Day;
        }
    }
}