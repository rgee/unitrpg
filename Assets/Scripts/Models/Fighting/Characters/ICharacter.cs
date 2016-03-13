using System;
using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting.Characters {
    public interface ICharacter {
        string Id { get; set;  }

        string Name { get; set; }

        int Level { get; set; }

        int Experience { get; set; }

        void AddExp(int amount);

        bool CanLevel();

        void LevelUp(IRandomizer randomizer);

        HashSet<Attribute> Attributes { get; set; }

        HashSet<Growth> Growths { get; set; }

        void AddToGrowth(Attribute.AttributeType type, int value);

        void AddToAttribute(Attribute.AttributeType type, int value);

        HashSet<Stat> Stats { get; set; } 

        HashSet<string> Weapons { get; set; } 

        HashSet<SkillType> Skills { get; set; } 

        HashSet<string> Inventory { get; set; } 

    }
}