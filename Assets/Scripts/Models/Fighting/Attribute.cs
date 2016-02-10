using System;

namespace Models.Fighting {
    public class Attribute {
        public enum AttributeType {
            Health,
            Strength,
            Speed,
            Skill,
            Defense,
            Kinesis,
            Special
        }

        public AttributeType Type { get; set; }
        public int Value { get; set; }
    }
}