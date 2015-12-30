namespace Combat.Interactive.Rules {
    public class DummyRule : ITileInteractivityRule {
        public bool CanBeUsed() {
            return true;
        }
    }
}