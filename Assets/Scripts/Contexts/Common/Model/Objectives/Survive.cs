using System;

namespace Contexts.Common.Model.Objectives {
    public class Survive : IObjective {
        public ObjectiveType Type {
            get {
                return ObjectiveType.Survive;
            }
        }

        public string Description {
            get {
                return string.Format("Survive {0} turns", _turns);
            }
        }

        private readonly int _turns;

        public Survive(int turns) {
            _turns = turns;
        }
    }
}