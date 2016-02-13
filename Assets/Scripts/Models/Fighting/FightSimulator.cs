using Models.Fighting.Equip;
using Models.Fighting.Skills;

namespace Models.Fighting {
    public class FightSimulator {
        private readonly IWeaponDatabase _weapons;
        
        public FightSimulator(IWeaponDatabase weapons) {
            _weapons = weapons;
        }
        
        public FightPreview SimulatePrimaryWeapon(ICombatant attacker, ICombatant defender) {
            var randomizer = new BasicRandomizer();
            var weapon = _weapons.GetByName(attacker.PrimaryWeapon);
            
            var ourStrategy = new MeleeAttack();
            var firstAttack = ourStrategy.Compute(attacker, defender, weapon, randomizer);

            var distance = MathUtils.ManhattanDistance(attacker.Position, defender.Position);
            var counterStrategy = defender.GetStrategyByDistance(distance);
            
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