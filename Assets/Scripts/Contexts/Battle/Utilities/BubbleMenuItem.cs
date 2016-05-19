using System.Collections.Generic;

namespace Contexts.Battle.Utilities {
    public class BubbleMenuItem {
        public readonly string Name;
        public readonly int Weight;
        public readonly HashSet<BubbleMenuItem> Children;

        private BubbleMenuItem(string name, int weight, HashSet<BubbleMenuItem> children) {
            Name = name;
            Weight = weight;
            Children = children;
        }

        public static BubbleMenuItem Leaf(string name, int weight) {
            return new BubbleMenuItem(name, weight, new HashSet<BubbleMenuItem>());
        }

        public static BubbleMenuItem Branch(string name, int weight, HashSet<BubbleMenuItem> children) {
            return new BubbleMenuItem(name, weight, children);
        }
    }
}