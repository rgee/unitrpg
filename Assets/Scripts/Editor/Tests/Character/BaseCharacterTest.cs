using System.Linq;
using Assets.Scripts.Editor;
using Models.Fighting;
using Models.Fighting.Characters;
using NUnit.Framework;
using WellFired.Shared;

namespace Tests.Character {
    [TestFixture]
    public class BaseCharacterTest {
        private static ICharacter GetCharacter() {
            return new CharacterBuilder()
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
        }

        [Test]
        public void TestLeveling() {
            var underTest = GetCharacter();
            var randomizer = new ConstantRandomizer(0);

            underTest.LevelUp(randomizer);
            var newAttrs = underTest.Attributes;
            newAttrs.Each((attr) => {
                Assert.AreEqual(2, attr.Value);
            });

            Assert.AreEqual(2, underTest.Level);
        }

        [Test]
        public void TestAddingToAttribute() {
            var underTest = GetCharacter();
            var originalValue = underTest.Attributes.First(attr => attr.Type == Attribute.AttributeType.Health);
            underTest.AddToAttribute(Attribute.AttributeType.Health, 1);

            var newValue = underTest.Attributes.First(attr => attr.Type == Attribute.AttributeType.Health);
            Assert.AreEqual(originalValue.Value + 1, newValue.Value);
        }

        [Test]
        public void TestAddingToGrowth() {
            
            var underTest = GetCharacter();
            var originalValue = underTest.Growths.First(attr => attr.Type == Attribute.AttributeType.Health);
            underTest.AddToGrowth(Attribute.AttributeType.Health, 1);

            var newValue = underTest.Growths.First(attr => attr.Type == Attribute.AttributeType.Health);
            Assert.AreEqual(originalValue.Value + 1, newValue.Value);
        }

        [Test]
        public void TestAddingExp() {
            
            var underTest = GetCharacter();
            underTest.AddExp(200);

            Assert.AreEqual(100, underTest.Experience);
            Assert.IsTrue(underTest.CanLevel());
        }
    }
}