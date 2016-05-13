namespace Models.Fighting {
    public static class RandomUtils {
        /// <summary>
        /// Roll the dice and find out if something happened based on integer percentage chances.
        /// </summary>
        /// <param name="percentChance">The percentage chance of an event happening.</param>
        /// <param name="randomizer">A randomizer.</param>
        /// <returns></returns>
        public static bool DidEventHappen(int percentChance, IRandomizer randomizer) {
            var roll = randomizer.GetNextRandom();
            return roll > (100 - percentChance);
        }
    }
}