using Contexts.Battle;

namespace Assets.Contexts.Chapters.Chapter2 {
    public class Chapter2Root : BattleRoot {
        void Awake() {
            context = new Chapter2Context(this);
        }
    }
}