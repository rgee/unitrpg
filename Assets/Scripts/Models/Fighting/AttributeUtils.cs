using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Buffs;
using Models.Fighting.Characters;

namespace Models.Fighting {
    public static class AttributeUtils {
        public static Attribute ApplyBuffs(Attribute attr, IEnumerable<IBuff> buffs) {
           foreach (var buff in buffs) {
               attr = buff.Apply(attr);
           }
           
           return attr; 
        }

        public static HashSet<Attribute.AttributeType> GetGrownTypes(HashSet<Growth> growths, IRandomizer randomizer) {
            return growths.Where(growth => {
                return growth.Value >= randomizer.GetNextRandom();
            })
            .Select(growth => {
                return growth.Type;
            })
            .ToHashSet();
        } 
    }
}