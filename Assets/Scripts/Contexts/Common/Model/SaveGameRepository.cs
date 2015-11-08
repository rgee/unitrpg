namespace Contexts.Common.Model {
    public class SaveGameRepository : ISaveGameRepository {
        public ISaveGameState CurrentGame { get; set; }

        [PostConstruct]
        public void Reset() {
            CurrentGame = new SaveGameState(0);            
        }
    }
}