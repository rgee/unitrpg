using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Models.Fighting.Maps.Configuration;
using Models.Fighting.Maps.Triggers;
using Newtonsoft.Json;
using UnityEngine;
using Utils;

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
        }

        public Map(int size) : this(size, size) {
        }

        public void AddEventTile(EventTile eventTile) {
            GetTileByPosition(eventTile.Location).Event = eventTile;
        }

        public void RemoveEventTile(Vector2 location) {
            GetTileByPosition(location).Event = null;
        }

        public EventTile GetEventTile(Vector2 location) {
            return GetTileByPosition(location).Event;
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
                return true;
            }

            var tile = GetTileByPosition(position);
            return tile.Obstructed;
        }

        public bool IsBlocked(Vector2 position) {
            if (!_tiles.ContainsKey(position)) {
                return true;
            }

            var tile = GetTileByPosition(position);
            return tile.Obstructed || tile.Occupant != null;
        }

        public List<ICombatant> GetAllOnMap() {
            return _tiles.Values
                .Where(tile => tile.Occupant != null)
                .Select(tile => tile.Occupant)
                .ToList();
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

        public HashSet<ICombatant> GetAdjacent(Vector2 position) {

            return MathUtils.GetAdjacentPoints(position)
                .Select(point => GetAtPosition(position))
                .ToHashSet();
        }

        public void MoveCombatant(ICombatant combatant, Vector2 position) {
            var destination = GetTileByPosition(position);
            if (destination.Occupant != null) {
                throw new ArgumentException("There's already a combatant " + "(" + destination.Occupant.Name + ")" + " at "+ position);
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

        private bool IsOnMap(Vector2 position) {
            return _tiles.ContainsKey(position);
        }

        public HashSet<Vector2> BreadthFirstSearch(Vector2 start, int maxDistance, bool ignoreOtherUnits) {
            var fringe = new Queue<Vector2>();            
            var results = new HashSet<Vector2>();
            var distances = new Dictionary<Vector2, int>();
            distances[start] = 1;
            
            fringe.Enqueue(start);
            while (fringe.Count > 0) {
                var current = fringe.Dequeue();
                if (distances[current] > maxDistance) {
                    break;
                }

                var currentDist = distances[current];

                var neighbors = MathUtils.GetAdjacentPoints(current);
                var openNeighbors = neighbors.Where((neighbor) => {
                    if (neighbor == start) {
                        return false;
                    }

                    if (!IsOnMap(neighbor)) {
                        return false;
                    }

                    if (ignoreOtherUnits) {
                        return !IsBlockedByEnvironment(neighbor);
                    }
                    
                    return !IsBlocked(neighbor);
                });
                
                foreach (var node in openNeighbors) {
                    if (!results.Contains(node)) {
                        fringe.Enqueue(node);
                        results.Add(node);
                        distances.Add(node, currentDist + 1);
                    }
                }
            }
            
            return results;
        }

        public List<Vector2> FindPath(Vector2 start, Vector2 goal) {
            if (start == goal) {
                return null;
            }

            if (IsBlocked(goal)) {
                return null;
            }

            var exactCosts = new Dictionary<Vector2, double>();
            var estimates = new Dictionary<Vector2, double>();
            var openNodes = new C5.IntervalHeap<Vector2>(new AStarComparer(exactCosts, estimates));

            var closedNodes = new HashSet<Vector2>();
            var path = new Dictionary<Vector2, Vector2>();

            openNodes.Add(start);
            exactCosts[start] = 0d;

            while (!openNodes.IsEmpty) {
                var currentCheapest = openNodes.DeleteMin();
                if (currentCheapest == goal) {
                    return ReconstructPath(path, currentCheapest);
                }

                closedNodes.Add(currentCheapest);
                var neighbors = MathUtils.GetAdjacentPoints(currentCheapest);
                foreach (var neighbor in neighbors) {
                    if (closedNodes.Contains(neighbor)) {
                        continue;
                    }

                    if (IsBlocked(neighbor)) {
                        continue;
                    }

                    var tentativeScore = exactCosts[currentCheapest] + CalculateDistance(currentCheapest, neighbor);
                    if (!estimates.ContainsKey(neighbor) || tentativeScore < estimates[neighbor]) {
                        var heuristicScore = tentativeScore + EstimateDistance(neighbor, goal);
                        estimates[neighbor] = heuristicScore;
                        exactCosts[neighbor] = tentativeScore;
                        path[neighbor] = currentCheapest;

                        openNodes.Add(neighbor);
                    }
                }
            }

            return null;
        }

        public List<Vector2> ReconstructPath(Dictionary<Vector2, Vector2> cameFrom, Vector2 current) {
            var result = new List<Vector2> {current};

            while (cameFrom.ContainsKey(current)) {
                current = cameFrom[current];
                result.Add(current);
            }

            result.Reverse();
            return result;
        } 

        private double EstimateDistance(Vector2 start, Vector2 end) {
            return MathUtils.ManhattanDistance(start, end);
        }

        private double CalculateDistance(Vector2 start, Vector2 end) {
            if (IsBlocked(end)) {
                return double.MaxValue;
            }

            return Vector2.Distance(start, end); 
        }
    }
}
