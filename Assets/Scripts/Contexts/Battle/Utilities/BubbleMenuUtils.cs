using System.Collections.Generic;
using Stateless;

namespace Contexts.Battle.Utilities {
    public class BubbleMenuUtils {
        
        public static StateMachine<string, string> CreateStateMachine(HashSet<BubbleMenuItem> items) {
            var sentinelStartValue = "level_0";
            var machine = new StateMachine<string, string>(sentinelStartValue);
            return CreateStateMachine(machine, sentinelStartValue, items);
        }

        private static StateMachine<string, string> CreateStateMachine(StateMachine<string, string> machine, string stateName, HashSet<BubbleMenuItem> items) {
            var config = machine.Configure(stateName);
            foreach (var bubble in items) {
                if (bubble.IsLeaf()) {
                    config = config.Permit(bubble.Name, "dispatch");
                } else {
                    config = config.Permit(bubble.Name, bubble.Name);
                    CreateStateMachine(machine, bubble.Name, bubble.Children);
                }
            }

            return machine;
        }
    }
}