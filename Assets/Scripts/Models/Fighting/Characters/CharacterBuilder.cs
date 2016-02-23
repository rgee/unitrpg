using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting.Characters {
    public class CharacterBuilder {
        private string _id;
        private string _name;
        private int _level = 1;
        private int _experience;
        private HashSet<Attribute> _attributes = new HashSet<Attribute>();
        private HashSet<Stat> _stats = new HashSet<Stat>();
        private HashSet<string> _weapons = new HashSet<string>();
        private HashSet<SkillType> _skills = new HashSet<SkillType>(); 
        private HashSet<Growth> _growths = new HashSet<Growth>(); 

        public CharacterBuilder Name(string name) {
            _name = name;
            return this;
        }

        public CharacterBuilder Attributes(HashSet<Attribute> attr) {
            _attributes = attr;
            return this;
        }

        public CharacterBuilder Stats(HashSet<Stat> stats) {
            _stats = stats;
            return this;
        }

        public CharacterBuilder Growths(HashSet<Growth> growths) {
            _growths = growths;
            return this;
        }

        public CharacterBuilder Skills(params SkillType[] skills) {
            _skills = skills.ToHashSet();
            return this;
        }

        public CharacterBuilder Weapons(params string[] weapons) {
            _weapons = weapons.ToHashSet();
            return this;
        }

        public CharacterBuilder Id(string id) {
            _id = id;
            return this;
        }
           
        public ICharacter Build() {
            return new BaseCharacter(_id, _name, _level, _experience, _attributes, _growths, _stats, _weapons, _skills);     
        } 
    }
}