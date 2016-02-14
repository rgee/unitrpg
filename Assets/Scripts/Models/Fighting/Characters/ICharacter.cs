﻿using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting.Characters {
    public interface ICharacter {
        string Id { get; set;  }

        string Name { get; set; }

        HashSet<Attribute> Attributes { get; set; }

        HashSet<Stat> Stats { get; set; } 

        HashSet<string> Weapons { get; set; } 

        HashSet<SkillType> Skills { get; set; } 
    }
}