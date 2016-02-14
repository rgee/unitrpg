﻿using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public class BaseCharacterDatabase : CharacterDatabase {
        private readonly Dictionary<string, ICharacter> _characters = new Dictionary<string, ICharacter>();

        public BaseCharacterDatabase() {
            Add(new CharacterBuilder()
                .Name("Liat")
                .Stats(new StatsBuilder().Leadership().Build())
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Weapons("Campaign Backblade", "Slim Recurve")
                .Build());

            Add(new CharacterBuilder()
                .Name("Janek")
                .Attributes(new AttributesBuilder()
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
                .Name("Maelle")
                .Attributes(new AttributesBuilder()
                    .Health(15)
                    .Skill(12)
                    .Defense(2)
                    .Special(0)
                    .Speed(13)
                    .Strength(7)
                .Build())
                .Weapons("Shortsword")
                .Build());
        }

        private void Add(ICharacter character) {
            _characters[character.Name] = character;
        }

        public ICharacter GetCharacter(string name) {
            return _characters[name];
        }
    }
}