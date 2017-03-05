namespace Models.Fighting.Maps.Triggers {
    public class TurnEvent {
        public readonly int ActivationTurn;
        public readonly string EventName;

        public TurnEvent(int activationTurn, string eventName) {
            ActivationTurn = activationTurn;
            EventName = eventName;
        }
    }
}