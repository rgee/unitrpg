using Pathfinding;

public static class PathfindingUtils {
    public static NNConstraint GetMainGraphConstraint() {
        var constraint = NNConstraint.None;
        constraint.graphMask = 1 << 0;

        return constraint;
    }

    public static NNConstraint GetUnitlessGraphConstraint() {
        var constraint = NNConstraint.None;
        constraint.graphMask = 1 << 1;

        return constraint;
    }
}