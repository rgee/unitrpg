using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public interface ICharacter {
        string Name { get; set; }

        HashSet<Attribute> Attributes { get; set; }
        HashSet<Stat> Stats { get; set; } 

        HashSet<string> Weapons { get; set; } 
    }
}