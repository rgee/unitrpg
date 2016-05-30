using System.Collections.Generic;
using System.IO;
using Models.Fighting.Characters;
using Models.Fighting.Skills;
using Models.SaveGames;
using NUnit.Framework;

namespace Tests.Saving {
    [TestFixture]
    public class DiskSaveGameRepositoryTest {
        [Test]
        public void TestSaving() {
            var path = TestContext.CurrentContext.TestDirectory;
            var repository = new DiskSaveGameRepository(path);

            var liat = new CharacterBuilder()
                .Id("liat")
                .Name("Liat")
                .Stats(new StatsBuilder().Leadership().Build())
                .Special(SkillType.Advance)
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
            var characters = new List<ICharacter> {liat};
            var save = new DefaultSaveGame(characters) {Path = Path.Combine(path, "test_save.json")};

            repository.Overwrite(save);
            var saves = repository.GetAllSaves();
            Assert.AreEqual(1, saves.Count);
        }
    }
}