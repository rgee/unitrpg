using System;

namespace Models.Fighting {
    public class Attribute {
        public enum AttributeType {
            Health,
            Strength,
            Speed,
            Skill,
            Defense,
            Special,
            Move
        }

        public AttributeType Type { get; set; }
        public int Value { get; set; }
    }
}