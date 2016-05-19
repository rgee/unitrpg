using System.Collections.Generic;
using Stateless;

namespace Contexts.Battle.Utilities {
    public class BubbleMenuUtils {

        public interface IMenuTransitionHandler {
            void Dispatch(string result);
            void ChangeLevel(string nextLevel);
        }

        public static StateMachine<string, string> CreateStateMachine(HashSet<BubbleMenuItem> items, IMenuTransitionHandler handler) {
            var sentinelStartValue = "base";
            var machine = new StateMachine<string, string>(sentinelStartValue);

            machine.Configure("dispatch")
                .OnEntry(transition => handler.Dispatch(transition.Trigger));

            return CreateStateMachine(machine, sentinelStartValue, null, items, handler);
        }

        private static StateMachine<string, string> CreateStateMachine(StateMachine<string, string> machine, string stateName, string parentState, HashSet<BubbleMenuItem> items,
            IMenuTransitionHandler handler) {
            var config = machine.Configure(stateName);
            config.OnEntry(() => handler.ChangeLevel(stateName));
            if (parentState != null) {
                config.Permit("back", parentState);
            }

            foreach (var bubble in items) {
                if (bubble.IsLeaf()) {
                    config = config.Permit(bubble.Name, "dispatch");
                } else {
                    config = config.Permit(bubble.Name, bubble.Name);
                    CreateStateMachine(machine, bubble.Name, stateName, bubble.Children, handler);
                }
            }

            return machine;
        }
    }
}