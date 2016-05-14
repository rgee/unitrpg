using System.Collections.Generic;
using Contexts.Battle.Models;
using UnityEngine;

namespace Contexts.Battle.Utilities {
    public class MapHighlights {
        public readonly HashSet<Vector2> Positions;
        public readonly HighlightLevel Level;

        public MapHighlights(HashSet<Vector2> positions, HighlightLevel level) {
            Positions = positions;
            Level = level;
        }
    }
}