using Models.SaveGames;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

namespace Assets.Contexts.SaveManagement.Common {
    public class SaveGameFileView : View {
        public Signal<ISaveGame> SelectedSignal = new Signal<ISaveGame>();
        public ISaveGame SaveGame { get; private set; }

        public void SetSaveGame(ISaveGame saveGame) {
            SaveGame = saveGame;
        }

        public void Select() {
            SelectedSignal.Dispatch(SaveGame);
        }
    }
}