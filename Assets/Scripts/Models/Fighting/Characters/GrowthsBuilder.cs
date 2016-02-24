using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public class GrowthsBuilder {
        private readonly HashSet<Growth> _attrs = new HashSet<Growth>();

        public GrowthsBuilder Health(int val) {
            AddAttribute(Attribute.AttributeType.Health, val);
            return this;
        }

        public GrowthsBuilder Strength(int val) {
            AddAttribute(Attribute.AttributeType.Strength, val);
            return this;
        }

        public GrowthsBuilder Speed(int val) {
            AddAttribute(Attribute.AttributeType.Speed, val);
            return this;
        }

        public GrowthsBuilder Skill(int val) {
            AddAttribute(Attribute.AttributeType.Skill, val);
            return this;
        }

        public GrowthsBuilder Defense(int val) {
            AddAttribute(Attribute.AttributeType.Defense, val);
            return this;
        }

        public GrowthsBuilder Special(int val) {
            AddAttribute(Attribute.AttributeType.Special, val);
            return this;
        }

        private void AddAttribute(Attribute.AttributeType type, int val) {
            _attrs.Add(new Growth {
                Type = type,
                Value = val
            });
        }

        public HashSet<Growth> Build() {
            return _attrs;
        }
    }
}