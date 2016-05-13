using Models.Combat;

namespace Models.Fighting {
    public interface IEffect {
        void Apply(ICombatant combatant);
    }
}