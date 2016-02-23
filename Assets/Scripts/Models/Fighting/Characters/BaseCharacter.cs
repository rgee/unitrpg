using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Skills;

namespace Models.Fighting.Characters {
    class BaseCharacter : ICharacter {
        private const int AttributeMax = 200;

        public string Id { get; set; }
        public string Name { get; set; }
        public HashSet<Attribute> Attributes { get; set; }
        public HashSet<Stat> Stats { get; set; }
        public HashSet<string> Weapons { get; set; }
        public HashSet<SkillType> Skills { get; set; }

        public BaseCharacter(string id, string name, HashSet<Attribute> attributes, HashSet<Stat> stats, HashSet<string> weapons, HashSet<SkillType> skills) {
            Id = id;
            Name = name;
            Attributes = attributes;
            Stats = stats;
            Weapons = weapons;
            Skills = skills;
        }

        public void AddToAttribute(Attribute.AttributeType type, int value) {
            Attributes = Attributes.Select((attribute => {
                if (attribute.Type == type) {
                    return new Attribute {
                        Type = type,
                        Value = Math.Min(AttributeMax, attribute.Value + value)
                    };
                }

                return attribute;
            })).ToHashSet();
        }
    }
}