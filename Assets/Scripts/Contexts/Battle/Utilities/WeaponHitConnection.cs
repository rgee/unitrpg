using Models.Fighting;
using Models.Fighting.Effects;

namespace Contexts.Battle.Utilities {
    public class WeaponHitConnection {
        public readonly WeaponHitSeverity Severity;
        public readonly ICombatant Combatant;

        public WeaponHitConnection(WeaponHitSeverity severity, ICombatant combatant) {
            Severity = severity;
            Combatant = combatant;
        }
    }
}