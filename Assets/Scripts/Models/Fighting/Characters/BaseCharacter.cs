using System.Collections.Generic;

namespace Models.Fighting.Characters {
    class BaseCharacter : ICharacter {
        public string Name { get; set; }
        public HashSet<Attribute> Attributes { get; set; }
        public HashSet<Stat> Stats { get; set; }
        public HashSet<string> Weapons { get; set; }

        public BaseCharacter(string name, HashSet<Attribute> attributes, HashSet<Stat> stats, HashSet<string> weapons) {
            Name = name;
            Attributes = attributes;
            Stats = stats;
            Weapons = weapons;
        }
    }
}