using Models.SaveGames;
using strange.extensions.signal.impl;

namespace Assets.Contexts.SaveManagement.Common {
    /// <summary>
    /// Dispatched when an already-existing Save Game slot is selected by the player.
    /// </summary>
    public class SaveGameSelectedSignal : Signal<ISaveGame> {
    }
}
