namespace Contexts.Common.Model {
    public interface IBattleConfigRepository {
        IBattleConfig GetConfigByIndex(int chapterIndex);
    }
}