using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public interface ICharacter {
        string Name { get; set; }

        HashSet<Attribute> Atributes { get; set; }
        HashSet<Stat> BaseStats { get; set; } 

        string WeaponName { get; set; }
        
        HashSet<AvailableSkill> Skills { get; set; } 
    }
}