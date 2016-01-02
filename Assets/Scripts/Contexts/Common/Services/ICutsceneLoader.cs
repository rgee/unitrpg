using Models.Dialogue;

namespace Assets.Contexts.Common.Services {
    public interface ICutsceneLoader {
        Cutscene Load(string resoucePath);
    }
}