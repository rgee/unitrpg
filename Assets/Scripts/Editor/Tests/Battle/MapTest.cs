using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NUnit.Framework;
using Models.Fighting.Maps;

namespace Tests.Battle {
    
    [TestFixture]
    public class MapTest {

        [Test]
        public void TestBFS() {
            var map = new Map(5);
            var result = map.BreadthFirstSearch(Vector2.zero, 1, true);
            Assert.AreEqual(3, result.Count);
        }
    }
}
