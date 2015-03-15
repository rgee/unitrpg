using UnityEngine;
using System.Collections;

public interface ResolutionStrategy {
	FightPhaseResult SimulateFightPhase(Participants participants, AttackType attack);
}
