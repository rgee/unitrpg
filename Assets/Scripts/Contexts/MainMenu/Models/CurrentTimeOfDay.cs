namespace Contexts.MainMenu.Models {
    public class CurrentTimeOfDay {
        public TimeOfDay Time { get; private set; }

        public CurrentTimeOfDay(TimeOfDay time) {
            Time = time;
        }
    }
}