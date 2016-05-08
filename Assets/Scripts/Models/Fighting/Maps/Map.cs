using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Models.Fighting.Maps {
    public class Map : IMap {
        private readonly Dictionary<Vector2, Tile> _tiles = new Dictionary<Vector2, Tile>(); 

        public Map() : this(20, 20) {
        }

        public Map(int width, int height) {
            for (var i = 0; i < width; i++) {
                for (var j = 0; j < height; j++) {
                    _tiles[new Vector2(i, j)] = new Tile();
                }
            }

            CombatEventBus.CombatantMoves.AddListener(MoveCombatant);
            CombatEventBus.CombatantDeaths.AddListener(RemoveCombatant);
        }

        public Map(int size) : this(size, size) {
        }

        public void AddObstruction(Vector2 position) {
            var tile = GetTileByPosition(position);
            if (tile.Occupant != null) {
                throw new ArgumentException("Cannot add an obstruction on an occupied tile.");
            }

            tile.Obstructed = true;
        }

        public bool IsBlockedByEnvironment(Vector2 position) {
            if (!_tiles.ContainsKey(position)) {
                return false;
            }

            var tile = GetTileByPosition(position);
            return tile.Obstructed;
        }

        public bool IsBlocked(Vector2 position) {
            if (!_tiles.ContainsKey(position)) {
                return false;
            }

            var tile = GetTileByPosition(position);
            return tile.Obstructed || tile.Occupant != null;
        }

        private Tile GetTileByPosition(Vector2 position) {
            if (!_tiles.ContainsKey(position)) {
                throw new ArgumentException("Position out of map bounds: " + position);
            }
            return _tiles[position];
        }

        public void RemoveCombatant(ICombatant combatant) {
            var tile = GetTileByPosition(combatant.Position);
            tile.Occupant = null;
        }

        public void AddCombatant(ICombatant combatant) {
            var position = combatant.Position;
            var tile = GetTileByPosition(position);
            if (tile.Occupant != null) {
                throw new ArgumentException("There's already a combatant at " + position);
            }
            tile.Occupant = combatant;
        }

        public void MoveCombatant(ICombatant combatant, Vector2 position) {
            var destination = GetTileByPosition(position);
            if (destination.Occupant != null) {
                throw new ArgumentException("There's already a combatant at "+ position);
            }

            if (destination.Obstructed) {
                throw new ArgumentException("Position " + position + " is not walkable.");
            }

            var source = GetTileByPosition(combatant.Position);
            source.Occupant = null;
            destination.Occupant = combatant;
            combatant.Position = position;
        }

        public ICombatant GetAtPosition(Vector2 position) {
            Tile result;
            _tiles.TryGetValue(position, out result);

            if (result == null) {
                return null;
            }

            return result.Occupant;
        }

        public HashSet<Vector2> BreadthFirstSearch(Vector2 start, int maxDistance, bool ignoreOtherUnits) {
            var fringe = new Queue<Vector2>();            
            var results = new HashSet<Vector2>();
            
            fringe.Enqueue(start);
            while (fringe.Count > 0) {
                var current = fringe.Dequeue();
                var neighbors = MathUtils.GetAdjacentPoints(current);
                var openNeighbors = neighbors.Where((neighbor) => { 
                    if (MathUtils.ManhattanDistance(start, neighbor) > maxDistance) {
                        return false;
                    }
                    
                    if (ignoreOtherUnits) {
                        return IsBlockedByEnvironment(neighbor);
                    } else {
                        return IsBlocked(neighbor);
                    }
                });
                
                foreach (var node in openNeighbors) {
                    fringe.Enqueue(node);
                    results.Add(node);  
                }
            }
            
            return results;
        }
    }
}