using System;
using System.Collections.Generic;
using System.Linq;

namespace Models.Fighting {
    public static class StatUtils {
        public static IEnumerable<Stat> ComputeStats(IEnumerable<Stat> stats, IEnumerable<IBuff> buffs) {
            var modLookup = buffs.SelectMany(buff => buff.StatMods)
                                 .ToLookup(pair => pair.Key, pair => pair.Value);

            return stats.Select(stat => {
                var mods = modLookup[stat.Type];
                var newVal = mods.Aggregate(stat.Value, (i, mod) => mod.Func(i));

                return new Stat(newVal, stat.Type);
            });
        } 
        
        public static Stat ApplyBuffs(Stat stat, IEnumerable<IBuff> buffs) {
            foreach (var buff in buffs) {
                stat = buff.Modify(stat);
            }
            
            return stat;
        }
    }
}