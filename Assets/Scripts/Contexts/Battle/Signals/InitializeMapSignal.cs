using Contexts.Battle.Utilities;
using Models.Fighting.Maps;
using strange.extensions.signal.impl;

namespace Contexts.Battle.Signals {
    /// <summary>
    /// Passes the width and height from the view layer (configured in-editor) up to the 
    /// business layer to create the Map on the model. This should be replaced
    /// with loading a JSON file for the map with obstacles and dimensions.
    /// </summary>
    public class InitializeMapSignal : Signal<MapConfiguration> {
    }
}
