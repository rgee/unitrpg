using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Skills;

namespace Models.Fighting.Characters {
    public class BaseCharacterDatabase : CharacterDatabase {
        public static CharacterDatabase Instance = new BaseCharacterDatabase();

        private readonly Dictionary<string, ICharacter> _characters = new Dictionary<string, ICharacter>();

        public BaseCharacterDatabase() {
            Add(new CharacterBuilder()
                .Id("liat")
                .Name("Liat")
                .Stats(new StatsBuilder().Leadership().Build())
                .Special(SkillType.Advance)
                .Attributes(new AttributesBuilder()
                    .Move(5)
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Weapons("Campaign Backblade")
                .Build());

            Add(new CharacterBuilder()
                .Id("janek")
                .Name("Janek")
                .Special(SkillType.Knockback)
                .Attributes(new AttributesBuilder()
                    .Move(5)
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Weapons("Chained Mace")
                .Build());

            Add(new CharacterBuilder()
                .Id("maelle")
                .Name("Maelle")
                .Attributes(new AttributesBuilder()
                    .Move(5)
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Weapons("Shortsword")
                .Build());

            Add(new CharacterBuilder()
                .Id("gatsu_chapter_1")
                .Name("Soldier")
                .Attributes(new AttributesBuilder()
                    .Move(5)
                    .Health(20)
                    .Skill(3)
                    .Speed(2)
                    .Defense(2)
                    .Special(0)
                    .Strength(8)
                .Build())
                .Weapons("Greatsword")
                .Build());

            Add(new CharacterBuilder()
                .Id("gatsu_chapter_2")
                .Name("Soldier")
                .Attributes(new AttributesBuilder()
                    .Move(5)
                    .Health(20)
                    .Skill(3)
                    .Speed(2)
                    .Defense(2)
                    .Special(0)
                    .Strength(8)
                .Build())
                .Weapons("Greatsword")
                .Build());

            Add(new CharacterBuilder()
                .Id("speedy_chapter_2")
                .Name("Soldier")
                .Attributes(new AttributesBuilder()
                    .Move(5)
                    .Health(20)
                    .Skill(3)
                    .Speed(2)
                    .Defense(2)
                    .Special(0)
                    .Strength(8)
                .Build())
                .Weapons("Greatsword")
                .Build());
        }

        private void Add(ICharacter character) {
            _characters[character.Id] = character;
        }

        public ICharacter GetCharacter(string id) {
            if (!_characters.ContainsKey(id)) {
                throw new ArgumentException("No character with id: " + id + " found.");
            }
            return _characters[id];
        }

        public List<ICharacter> GetAllCharacters() {
            return _characters.Values.ToList();
        }
    }
}