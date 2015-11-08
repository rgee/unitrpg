namespace Contexts.Common.Model {
    public class SaveGameState : ISaveGameState {
        public SaveGameState(int chapter) {
            Chapter = chapter;
        }

        public int Chapter { get; private set; }
    }
}