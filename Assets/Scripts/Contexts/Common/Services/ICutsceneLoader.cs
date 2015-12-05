namespace Assets.Contexts.Common.Services {
    public interface ICutsceneLoader {
        Models.Dialogue.Cutscene Load(string resoucePath);
    }
}