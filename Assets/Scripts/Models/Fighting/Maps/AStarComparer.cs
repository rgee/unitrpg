using System.Collections.Generic;
using UnityEngine;

namespace Models.Fighting.Maps {
    class AStarComparer : IComparer<Vector2> {
        private readonly Dictionary<Vector2, double> exactCosts;

        private readonly Dictionary<Vector2, double> estimatedCosts;

        public AStarComparer(Dictionary<Vector2, double> exactCosts, Dictionary<Vector2, double> estimatedCosts) {
            this.exactCosts = exactCosts;
            this.estimatedCosts = estimatedCosts;
        }

        public int Compare(Vector2 left, Vector2 right) {
            var leftCost = GetHeuristicScore(left);
            var rightCost = GetHeuristicScore(right);
            if (leftCost < rightCost) {
                return -1;
            }

            if (leftCost > rightCost) {
                return 1;
            }

            return 0;
        }

        private double GetHeuristicScore(Vector2 vec) {
            return exactCosts[vec] + estimatedCosts[vec];
        }
    }
}