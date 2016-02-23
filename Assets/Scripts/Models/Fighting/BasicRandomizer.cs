using System;

namespace Models.Fighting {
    public class BasicRandomizer : IRandomizer {
        private readonly Random _rand = new Random();
        public int GetNextRandom() {
            return _rand.Next(0, 100);
        }
    }
}