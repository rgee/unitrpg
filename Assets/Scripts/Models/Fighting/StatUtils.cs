using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Fighting {
    public static class StatUtils {
        private class ModdedStat : IStat {
            public int Value { get; private set; }
            public StatType Type { get; private set; }

            public ModdedStat(int value, StatType type) {
                Value = value;
                Type = type;
            }
        }

        public static IEnumerable<IStat> ComputeStats(IEnumerable<IStat> stats, IEnumerable<IBuff> buffs) {
            var modLookup = buffs.SelectMany(buff => buff.StatMods)
                                 .ToLookup(pair => pair.Key, pair => pair.Value);

            return stats.Select(stat => {
                var mods = modLookup[stat.Type];
                var newVal = mods.Aggregate(stat.Value, (i, mod) => mod.Func(i));

                return new ModdedStat(newVal, stat.Type) as IStat;
            });
        } 
    }
}