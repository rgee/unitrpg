using System.Collections.Generic;
using System.Linq;
using Models.Fighting.Effects;

namespace Models.Fighting.Skills {
    public class SkillEffects {
        public readonly List<IEffect> ReceiverEffects;

        public int AttackCount {
            get { return ReceiverEffects.OfType<Damage>().Count(); }
        }

        public WeaponHitSeverity Severity {
            get {
                var weaponHits = ReceiverEffects.OfType<WeaponHit>().ToList();
                if (!weaponHits.Any() || weaponHits.OfType<Miss>().Any()) {
                    return WeaponHitSeverity.Miss;
                }

                if (weaponHits.OfType<WeaponCrit>().Any()) {
                    return WeaponHitSeverity.Crit;
                }

                if (weaponHits.OfType<WeaponGlance>().Any()) {
                    return WeaponHitSeverity.Glance;
                }

                return WeaponHitSeverity.Normal;
            }
        }

        public SkillEffects(List<IEffect> receiverEffects) {
            ReceiverEffects = receiverEffects;
        }

        public SkillEffects() {
            ReceiverEffects = new List<IEffect>();
        }

        public int GetDefenderDamage() {
            return ReceiverEffects.OfType<Damage>().Sum(dmgEffect => dmgEffect.Amount);
        }
    }
}