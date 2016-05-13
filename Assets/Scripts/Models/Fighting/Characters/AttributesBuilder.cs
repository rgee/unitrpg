using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public class AttributesBuilder {
        private readonly HashSet<Attribute> _attrs = new HashSet<Attribute>();

        public AttributesBuilder Move(int val) {
            AddAttribute(Attribute.AttributeType.Move, val);
            return this;
        }

        public AttributesBuilder Health(int val) {
            AddAttribute(Attribute.AttributeType.Health, val);
            return this;
        }

        public AttributesBuilder Strength(int val) {
            AddAttribute(Attribute.AttributeType.Strength, val);
            return this;
        }

        public AttributesBuilder Speed(int val) {
            AddAttribute(Attribute.AttributeType.Speed, val);
            return this;
        }

        public AttributesBuilder Skill(int val) {
            AddAttribute(Attribute.AttributeType.Skill, val);
            return this;
        }

        public AttributesBuilder Defense(int val) {
            AddAttribute(Attribute.AttributeType.Defense, val);
            return this;
        }

        public AttributesBuilder Special(int val) {
            AddAttribute(Attribute.AttributeType.Special, val);
            return this;
        }

        private void AddAttribute(Attribute.AttributeType type, int val) {
            _attrs.Add(new Attribute {
                Type = type,
                Value = val
            });
        }

        public HashSet<Attribute> Build() {
            return _attrs;
        } 
    }
}