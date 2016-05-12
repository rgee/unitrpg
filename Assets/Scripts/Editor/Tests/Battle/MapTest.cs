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

        [Test]
        public void TestEasySearch() {
            var map = new Map(5);
            var start = Vector2.zero;
            var end = new Vector2(0, 1);
            var path = map.FindPath(start, end);

            Assert.NotNull(path);
            Assert.AreEqual(2, path.Count);
        }

        [Test]
        public void TestBlockedSearch() {
            var map = new Map(5);
            var start = Vector2.zero;
            var end = new Vector2(2, 0);
            map.AddObstruction(new Vector2(1, 0));
            var path = map.FindPath(start, end);

            Assert.NotNull(path);
            var desiredPath = new List<Vector2> {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(2, 1),
                new Vector2(2, 0)
            };
            CollectionAssert.AreEqual(desiredPath, path);
        }

        [Test]
        public void TestTrivialSearch() {
            
            var map = new Map(5);
            var start = Vector2.zero;
            var path = map.FindPath(start, start);

            Assert.Null(path);
        }

        [Test]
        public void TestGoalObstructed() {
            
            var map = new Map(5);
            var start = Vector2.zero;
            var end = new Vector2(2, 0);
            map.AddObstruction(new Vector2(2, 0));
            var path = map.FindPath(start, end);

            Assert.Null(path);
        }
    }
}
