using Models.Fighting;

namespace Assets.Scripts.Editor {
    public class ConstantRandomizer : IRandomizer {
        private int _constant;

        public ConstantRandomizer(int constant) {
            _constant = constant;
        }

        public int GetNextRandom() {
            return _constant;
        }
    }
}