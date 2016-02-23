using System;
using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Skills;
using WellFired.Shared;

namespace Models.Fighting.Characters {
    class BaseCharacter : ICharacter {
        private const int AttributeMax = 200;
        private const int GrowthMax = 100;

        public string Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int Experience { get; set; }
        public HashSet<Attribute> Attributes { get; set; }
        public HashSet<Growth> Growths { get; set; } 
        public HashSet<Stat> Stats { get; set; }
        public HashSet<string> Weapons { get; set; }
        public HashSet<SkillType> Skills { get; set; }

        public BaseCharacter(string id, string name, int level, int experience, HashSet<Attribute> attributes, HashSet<Growth> growths, HashSet<Stat> stats, HashSet<string> weapons, HashSet<SkillType> skills) {
            Id = id;
            Level = level;
            Name = name;
            Experience = experience;
            Attributes = attributes;
            Growths = growths;
            Stats = stats;
            Weapons = weapons;
            Skills = skills;
        }

        public void LevelUp(IRandomizer randomizer) {
            Experience = 0;
            Level = Level + 1;
            AttributeUtils.GetGrownTypes(Growths, randomizer).Each(type => {
                AddToAttribute(type, 1);
            });
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

        public void AddToGrowth(Attribute.AttributeType type, int value) {
            Growths = Growths.Select((growth => {
                if (growth.Type == type) {
                    return new Growth {
                        Type = type,
                        Value = Math.Min(GrowthMax, growth.Value + value)
                    };
                }

                return growth;
            })).ToHashSet();
        }
    }
}