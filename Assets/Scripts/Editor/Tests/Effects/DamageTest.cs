using Models.Fighting;
using Models.Fighting.Characters;
using Models.Fighting.Effects;
using NUnit.Framework;

namespace Tests.Effects {
    [TestFixture]
    public class DamageTest {

        [Test]
        public void TakeDamage() {
            var barrelCharacter = new CharacterBuilder()
                .Attributes(new AttributesBuilder()
                        .Health(50)
                        .Defense(0)
                    .Build())
                .Build();
            var fuckingBarrel = new BaseCombatant(barrelCharacter, ArmyType.Other);
            var damage = new Damage(100);

            damage.Apply(fuckingBarrel);
            Assert.AreEqual(0, fuckingBarrel.Health);
            Assert.IsFalse(fuckingBarrel.IsAlive);
        }
    }
}