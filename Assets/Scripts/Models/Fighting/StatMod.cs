using System;

namespace Models.Fighting {
    public class StatMod {
        public Func<int, int> Func;
        public string Source;

        public StatMod(string source, Func<int, int> func) {
            Func = func;
            Source = source;
        }
    }
}