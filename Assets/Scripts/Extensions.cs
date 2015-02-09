using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class Extensions {
    public static HashSet<T> ToHashSet<T>(this IEnumerable<T> query) {
        return new HashSet<T>(query);
    }
}
