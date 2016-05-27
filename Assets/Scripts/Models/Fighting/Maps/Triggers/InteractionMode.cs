namespace Models.Fighting.Maps.Triggers {
    /// <summary>
    /// The method by which a friendly unit must interact with an EventTile 
    /// to trigger it.
    /// </summary>
    public enum InteractionMode {
        // Just land on the tile 
        Walk, 

        // Must use it like an action
        Use
    }
}