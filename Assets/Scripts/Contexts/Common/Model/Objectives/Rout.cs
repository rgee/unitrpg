namespace Contexts.Common.Model.Objectives {
    public class Rout : IObjective {
        public ObjectiveType Type {
            get {
                return ObjectiveType.Rout;
            }
        }

        public string Description {
            get {
                return _turnLimit == null
                    ? "Kill all enemies."
                    : string.Format("Kill all enemies in {0} turns", _turnLimit);
            }
        }

        private int? _turnLimit;

        public Rout(int turnLimit) {
            _turnLimit = turnLimit;
        }

        public Rout() {
            
        }
    }
}