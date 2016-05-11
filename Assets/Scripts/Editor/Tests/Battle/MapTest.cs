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
            map.AddObstruction(new Vector2(1, 0));
            var result = map.BreadthFirstSearch(Vector2.zero, 1, true);
            Assert.AreEqual(2, result.Count);
        }
    }
}
