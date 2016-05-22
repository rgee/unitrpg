using System.Collections.Generic;

namespace Contexts.Battle.Utilities {
    public class BubbleMenuItem {
        public readonly string Name;
        public readonly int Weight;
        public readonly HashSet<BubbleMenuItem> Children;
        public readonly string ResourcePath;

        private BubbleMenuItem(string name, int weight, HashSet<BubbleMenuItem> children, string resourcePath) {
            Name = name;
            Weight = weight;
            Children = children;
            ResourcePath = resourcePath;
        }

        public bool IsLeaf() {
            return Children == null || Children.Count <= 0;
        }

        public static BubbleMenuItem Leaf(string name, int weight, string resourcePath = null) {
            return new BubbleMenuItem(name, weight, new HashSet<BubbleMenuItem>(), resourcePath);
        }

        public static BubbleMenuItem Branch(string name, int weight, HashSet<BubbleMenuItem> children, string resourcePath = null) {
            return new BubbleMenuItem(name, weight, children, resourcePath);
        }
    }
}