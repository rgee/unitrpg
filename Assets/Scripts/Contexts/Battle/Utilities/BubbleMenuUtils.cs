using System.Collections.Generic;
using Contexts.BattlePrep.Signals;
using Grid;
using Models.Combat;
using strange.extensions.signal.impl;
using Stateless;
using UI.ActionMenu.Bubbles;

namespace Contexts.Battle.Utilities {
    public class BubbleMenuUtils {

        private static readonly HashSet<CombatActionType> FightActions = new HashSet<CombatActionType> {
            CombatActionType.Attack,
            CombatActionType.Brace
        };

        private static readonly Dictionary<CombatActionType, int> ActionWeights = new Dictionary<CombatActionType, int> {
            { CombatActionType.Use, 1 },
            { CombatActionType.Fight, 2 },
            { CombatActionType.Move, 3 },
            { CombatActionType.Item, 4 },
            { CombatActionType.Trade, 5 },
            { CombatActionType.Talk, 6 },
            { CombatActionType.Attack, 7 },
            { CombatActionType.Brace, 8 },
            { CombatActionType.Cover, 9 }
        };

        public interface IMenuTransitionHandler {
            void Dispatch(string result);
            void ChangeLevel(string nextLevel);
        }

        public class MenuStateMachine {
            public readonly Signal<StateMachine<string, string>.Transition> ChangeLevelSignal;
            public readonly Signal<string> SelectSignal;
            public readonly Signal CloseSignal;

            public string State {
                get { return _machine.State; }
            }

            private readonly StateMachine<string, string> _machine;

            public MenuStateMachine(Signal<StateMachine<string, string>.Transition> changeLevelSignal, Signal closeSignal, Signal<string> selectSignal, StateMachine<string, string> machine) {
                CloseSignal = closeSignal;
                ChangeLevelSignal = changeLevelSignal;
                SelectSignal = selectSignal;
                _machine = machine;
            }

            public void Fire(string trigger) {
                _machine.Fire(trigger);
            }

            public void GoBack() {
                _machine.Fire("Back");
            }
        }


        public static MenuStateMachine CreateStateMachine(HashSet<BubbleMenuItem> items) {
            var sentinelStartValue = "base";
            var machine = new StateMachine<string, string>(sentinelStartValue);
            var selectSignal = new Signal<string>();
            var deepenSignal = new Signal<StateMachine<string, string>.Transition>();
            var closeSignal = new Signal();

            machine.Configure("dispatch")
                .OnEntry(transition => selectSignal.Dispatch(transition.Trigger));

            machine.Configure("base")
                .Permit("Back", "closed");

            machine.Configure("closed")
                .OnEntry(transition => closeSignal.Dispatch());

            machine = CreateStateMachine(machine, sentinelStartValue, null, items, deepenSignal);

            return new MenuStateMachine(deepenSignal, closeSignal, selectSignal, machine);
        }

        private static StateMachine<string, string> CreateStateMachine(StateMachine<string, string> machine, string stateName, string parentState, HashSet<BubbleMenuItem> items,
            Signal<StateMachine<string, string>.Transition> deepenSignal) {
            var config = machine.Configure(stateName);
            config.OnEntry((transition) => deepenSignal.Dispatch(transition));
            if (parentState != null) {
                config.Permit("Back", parentState);
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

        public static HashSet<BubbleMenuItem> CreateFromActions(HashSet<CombatActionType> actionTypes) {
            var result = new HashSet<BubbleMenuItem>();
            var fightItems = new HashSet<BubbleMenuItem>();

            foreach (var action in actionTypes) {
                var item = BubbleMenuItem.Leaf(action.ToString(), ActionWeights[action]);
                if (FightActions.Contains(action)) {
                    fightItems.Add(item);
                } else {
                    result.Add(item);
                }
            }

            if (fightItems.Count > 0) {
                result.Add(BubbleMenuItem.Branch("Fight", ActionWeights[CombatActionType.Fight], fightItems));
            }
            return result;
        }
    }
}