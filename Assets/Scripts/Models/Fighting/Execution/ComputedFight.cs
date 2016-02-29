namespace Models.Fighting.Execution {
    /// <summary>
    /// This is a fight that has already been rolled for, and everything has been computed.
    /// </summary>
    public class ComputedFight {
        /// <summary>
        /// The phase that initiates the fight
        /// </summary>
        public ComputedFightPhase InitialPhase { get; set; }

        /// <summary>
        /// The phase where the flanker attacks the target too. (optional)
        /// </summary>
        public ComputedFightPhase FlankerPhase { get; set; }

        /// <summary>
        /// The phase where the receiver delivers a counter action. (optional)
        /// </summary>
        public ComputedFightPhase CounterPhase { get; set; }

        /// <summary>
        /// The phase where the initiator attacks again. (optional)
        /// </summary>
        public ComputedFightPhase DoubleAttackPhase { get; set; }
    }
}