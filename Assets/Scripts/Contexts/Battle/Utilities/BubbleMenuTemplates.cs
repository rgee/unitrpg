using System.Collections.Generic;

namespace Contexts.Battle.Utilities {
    public class BubbleMenuTemplates {
        public static HashSet<BubbleMenuItem> GetContextMenuTemplate() {
            return new HashSet<BubbleMenuItem> {
                BubbleMenuItem.Branch("System", 0, new HashSet<BubbleMenuItem> {
                    BubbleMenuItem.Leaf("Reset", 0),
                    BubbleMenuItem.Leaf("Suspend", 1),
                    BubbleMenuItem.Leaf("Options", 2),
                    BubbleMenuItem.Leaf("Tips", 3)
                }),
                BubbleMenuItem.Leaf("Tactical", 1),
                BubbleMenuItem.Leaf("Units", 2),
                BubbleMenuItem.Branch("Range", 3, new HashSet<BubbleMenuItem> {
                    BubbleMenuItem.Leaf("Off", 0),
                    BubbleMenuItem.Leaf("Attack", 1),
                    BubbleMenuItem.Leaf("Absolute", 2)
                }),
                BubbleMenuItem.Leaf("End Turn", 4)
            };
        }
    }
}