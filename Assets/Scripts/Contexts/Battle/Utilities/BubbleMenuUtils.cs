﻿using System.Collections.Generic;
using Grid;
using strange.extensions.signal.impl;
using Stateless;

namespace Contexts.Battle.Utilities {
    public class BubbleMenuUtils {

        public interface IMenuTransitionHandler {
            void Dispatch(string result);
            void ChangeLevel(string nextLevel);
        }

        public class MenuStateMachine {
            public readonly Signal<string> ChangeLevelSignal;
            public readonly Signal<string> SelectSignal;

            public string State {
                get { return _machine.State; }
            }

            private readonly StateMachine<string, string> _machine;

            public MenuStateMachine(Signal<string> changeLevelSignal, Signal<string> selectSignal, StateMachine<string, string> machine) {
                ChangeLevelSignal = changeLevelSignal;
                SelectSignal = selectSignal;
                _machine = machine;
            }

            public void Fire(string trigger) {
                _machine.Fire(trigger);
            }

            public void GoBack() {
                _machine.Fire("back");
            }
        }


        public static MenuStateMachine CreateStateMachine(HashSet<BubbleMenuItem> items) {
            var sentinelStartValue = "base";
            var machine = new StateMachine<string, string>(sentinelStartValue);
            var selectSignal = new Signal<string>();
            var deepenSignal = new Signal<string>();

            machine.Configure("dispatch")
                .OnEntry(transition => selectSignal.Dispatch(transition.Trigger));

            machine = CreateStateMachine(machine, sentinelStartValue, null, items, deepenSignal);

            return new MenuStateMachine(deepenSignal, selectSignal, machine);
        }

        private static StateMachine<string, string> CreateStateMachine(StateMachine<string, string> machine, string stateName, string parentState, HashSet<BubbleMenuItem> items,
            Signal<string> deepenSignal) {
            var config = machine.Configure(stateName);
            config.OnEntry(() => deepenSignal.Dispatch(stateName));
            if (parentState != null) {
                config.Permit("back", parentState);
            }

            foreach (var bubble in items) {
                if (bubble.IsLeaf()) {
                    config = config.Permit(bubble.Name, "dispatch");
                } else {
                    config = config.Permit(bubble.Name, bubble.Name);
                    CreateStateMachine(machine, bubble.Name, stateName, bubble.Children, deepenSignal);
                }
            }

            return machine;
        }
    }
}