namespace Models.Fighting {
    public static class RandomUtils {
        public static bool DidEventHappen(int percentChance, IRandomizer randomizer) {
            var roll = randomizer.GetNextRandom();
            return roll <= percentChance;
        }
    }
}