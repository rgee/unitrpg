using System.Collections.Generic;

namespace Models.Fighting.Characters {
    public class StatsBuilder {
        private const int DefaultLeadershipRange = 3;
        private readonly HashSet<Stat> _stats = new HashSet<Stat>();

        public StatsBuilder ParryChance(int val) {
            AddStat(StatType.MoveRange, val);
            return this;
        }

        public StatsBuilder Leadership(int range = DefaultLeadershipRange) {
            AddStat(StatType.LeadershipRange, range);
            return this;
        }

        private void AddStat(StatType type, int val) {
            _stats.Add(new Stat(val, type));
        }

        public HashSet<Stat> Build() {
            return _stats;
        } 
    }
}