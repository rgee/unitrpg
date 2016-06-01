using System.Collections.Generic;
using Contexts.Global.Models;
using Models.SaveGames;

namespace Contexts.Global.Services {
    public interface ISaveGameService {
        ISaveGame CurrentSave { get; }

        /// <summary>
        /// Create the default, starting Save Game and set it as current.
        /// </summary>
        void CreateNewGame();

        /// <summary>
        /// Set a save game as the current one
        /// </summary>
        /// <param name="saveGame">The save game to make current</param>
        void Choose(ISaveGame saveGame);

        /// <summary>
        /// Persist a new save game.
        /// </summary>
        /// <param name="saveGame">The save game to persist</param>
        void Write(ISaveGame saveGame);

        /// <summary>
        /// Overwrite a save game with a new one.
        /// </summary>
        /// <param name="existingSave">The save game to overwrite</param>
        /// <param name="newSave">The save game to persist</param>
        void Overwrite(ISaveGame existingSave, ISaveGame newSave);

        /// <summary>
        /// Get all available Save games
        /// </summary>
        /// <returns>A List of all save games</returns>
        List<ISaveGame> GetAll();
    }
}