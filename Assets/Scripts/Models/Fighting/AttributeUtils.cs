using System.Collections.Generic;
using System.Linq;

namespace Models.Fighting {
    public static class AttributeUtils {
        public static Attribute ApplyBuffs(Attribute attr, IEnumerable<IBuff> buffs) {
           foreach (var buff in buffs) {
               attr = buff.Apply(attr);
           }
           
           return attr; 
        }
    }
}