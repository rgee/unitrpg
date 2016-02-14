﻿using System.Collections.Generic;
using Models.Fighting.Skills;

namespace Models.Fighting.Characters {
    public class CharacterBuilder {
        private string _name;
        private HashSet<Attribute> _attributes;
        private HashSet<Stat> _stats;
        private HashSet<string> _weapons;
        private HashSet<SkillType> _skills; 

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

        public CharacterBuilder Skills(params SkillType[] skills) {
            _skills = skills.ToHashSet();
            return this;
        }

        public CharacterBuilder Weapons(params string[] weapons) {
            _weapons = weapons.ToHashSet();
            return this;
        }
           
        public ICharacter Build() {
            return new BaseCharacter(_name, _attributes, _stats, _weapons);     
        } 
    }
}