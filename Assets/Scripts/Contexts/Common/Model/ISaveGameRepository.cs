namespace Contexts.Common.Model {
    public interface ISaveGameRepository {
        ISaveGameState CurrentGame { get; set; }
        void Reset();
    }
}