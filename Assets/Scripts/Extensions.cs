using System.Collections.Generic;

public static class Extensions {
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> query) {
        return new HashSet<T>(query);
    }
}