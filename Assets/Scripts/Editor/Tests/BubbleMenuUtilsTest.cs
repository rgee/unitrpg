﻿using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Utilities;
using NUnit.Framework;

namespace Tests {
    [TestFixture]
    public class BubbleMenuUtilsTest {
        [Test]
        public void TestLeafTermination() {
            var config = new HashSet<BubbleMenuItem> {
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

            var machine = BubbleMenuUtils.CreateStateMachine(config);
            machine.Fire("Tactical");
            Assert.AreEqual("dispatch", machine.State);
        }

        [Test]
        public void TestSubMenuTransition() {
            var config = new HashSet<BubbleMenuItem> {
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

            var machine = BubbleMenuUtils.CreateStateMachine(config);
            var initialState = machine.State;
            machine.Fire("System");
            Assert.AreEqual("System", machine.State);

            machine.GoBack();
            Assert.AreEqual(initialState, machine.State);

            machine.Fire("System");
            machine.Fire("Reset");

            Assert.AreEqual("dispatch", machine.State);
        }
    }
}