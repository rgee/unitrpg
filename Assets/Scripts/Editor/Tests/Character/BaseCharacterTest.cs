using Assets.Scripts.Editor;
using Models.Fighting.Characters;
using NUnit.Framework;
using WellFired.Shared;

namespace Tests.Character {
    [TestFixture]
    public class BaseCharacterTest {
        [Test]
        public void TestLeveling() {
            var underTest = new CharacterBuilder()
                .Growths(new GrowthsBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                    .Build())
                .Attributes(new AttributesBuilder()
                    .Health(1)
                    .Skill(1)
                    .Defense(1)
                    .Special(1)
                    .Speed(1)
                    .Strength(1)
                    .Build())
                .Weapons("Shortsword")
                .Build();
            var randomizer = new ConstantRandomizer(0);

            underTest.LevelUp(randomizer);
            var newAttrs = underTest.Attributes;
            newAttrs.Each((attr) => {
                Assert.AreEqual(2, attr.Value);
            });


            Assert.AreEqual(2, underTest.Level);
        }
    }
}