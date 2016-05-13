namespace Models.Fighting.Execution {
    /// <summary>
    /// A fight receiver's response to a single combat phase.
    /// </summary>
    public enum DefenderResponse {
        /// <summary>
        /// The defender is hit by the attack (or heal)
        /// </summary>
        GetHit,

        /// <summary>
        /// The defender dodges the action.
        /// </summary>
        Dodge,

        /// <summary>
        /// The defender parries. Projectiles only.
        /// </summary>
        Parry
    }
}