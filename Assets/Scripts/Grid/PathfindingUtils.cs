using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public static class PathfindingUtils {
    public static Pathfinding.NNConstraint GetMainGraphConstraint() {
        Pathfinding.NNConstraint constraint = Pathfinding.NNConstraint.None;
        constraint.graphMask = 1 << 0;

        return constraint;
    }

    public static Pathfinding.NNConstraint GetUnitlessGraphConstraint() {
        Pathfinding.NNConstraint constraint = Pathfinding.NNConstraint.None;
        constraint.graphMask = 1 << 1;

        return constraint;
    }
}