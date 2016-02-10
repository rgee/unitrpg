using System;
using System.Collections.Generic;
using Models.Fighting;
using Models.Fighting.Effects;
using NUnit.Framework;

namespace Tests.Effects {
    [TestFixture]
    public class DamageTest {

        private class FuckingBarrel : BaseCombatant {
            public FuckingBarrel(int health) {
                Health = health;
            }
        }

        [Test]
        public void TakeDamage() {
            ICombatant barrel = new FuckingBarrel(50);
            IEffect damage = new Damage(100);

            damage.Apply(barrel);
            Assert.AreEqual(0, barrel.Health);
            Assert.IsFalse(barrel.IsAlive);
        }
    }
}