using Models.Fighting.Equip;
using Models.Fighting.Skills;

namespace Models.Fighting {
    public class FightSimulator {
        private readonly IWeaponDatabase _weapons;
        
        public FightSimulator(IWeaponDatabase weapons) {
            _weapons = weapons;
        }
        
        public FightPreview SimulatePrimaryWeapon(ICombatant attacker, ICombatant defender) {
            var weapon = _weapons.GetByName(attacker.PrimaryWeapon);
            foreach (var receiverBuff in weapon.ReceiverPreCombatBuffs) {
               defender.AddTemporaryBuff(receiverBuff) ;
            }
            
            var skill = new Skill(attacker, defender, weapon.ReceiverPreCombatBuffs, weapon.ReceiverOnHitBuffs);
            
            foreach (var receiverBuff in weapon.ReceiverPreCombatBuffs) {
                defender.RemoveTemporaryBuff(receiverBuff);
            }
            
            return null;
        }
        
        public FightPreview SimulateSecondaryWeapon(ICombatant attacker, ICombatant defender) {
            return null;
        }
        
        public FightPreview SimulateKinesis(ICombatant attacker, ICombatant defender) {
            return null;
        }
        
        public FightPreview SimulateHeal(ICombatant attacker, ICombatant defender) {
            return null;
        }
    }
}