using System;
using System.Collections.Generic;
using UnityEngine;

namespace Models.Fighting.Execution {
    public class FightExecutor {
        public void Execute(FinalizedFight fight) {
            var phases = new List<FightPhase>() {
                fight.InitialPhase,
                fight.FlankerPhase,
                fight.CounterPhase,
                fight.DoubleAttackPhase
            };
            phases.RemoveAll(phase => phase == null);

            phases.ForEach(phase => {
                var receiver = phase.Receiver;
                var effects = phase.Effects.ReceiverEffects;
                var damage = phase.Effects.GetDefenderDamage();
                var initiator = phase.Initiator;
                var maxHealth = receiver.GetAttribute(Attribute.AttributeType.Health).Value;
                var remainingHealth = Math.Max(0, receiver.Health - damage);
                Debug.LogFormat("{0} did {1} damage to {2}.\nHP: {3}/{4}", initiator.Id, damage, receiver.Id, 
                    remainingHealth, maxHealth);

                effects.ForEach(effect => {
                    effect.Apply(receiver);
                });
            });
        } 
    }
}