using System;
using Random = UnityEngine.Random;

namespace Models.Fighting.Skills {
    public class BasicRandomizer : IRandomizer {
        public int GetNextRandom() {
            return (int) Math.Floor(Random.value);
        }
    }
}