using System;

namespace Models.Fighting.Maps {
    public static class PathfindingUtils {
        public static Predicate<Tile> GetCombatantTileFilter(ICombatant pathfinder) {
            return tile => {
                if (tile.Obstructed) {
                    return false;
                }

                var occupant = tile.Occupant;
                return occupant == null || occupant.Army == pathfinder.Army;
            };
        } 
    }
}