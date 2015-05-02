public interface ResolutionStrategy {
    FightPhaseResult SimulateFightPhase(Participants participants, AttackType attack);
}