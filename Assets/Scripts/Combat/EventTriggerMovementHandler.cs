using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Models.Combat;
using UnityEngine;

namespace Combat {
    public class EventTriggerMovementHandler : IMovementEventHandler {

        private readonly BattleManager _battle;

        public EventTriggerMovementHandler(BattleManager battle) {
            this._battle = battle;
        }


        public IEnumerator HandleMovement(Grid.Unit unit, Vector2 destination) {
            yield return _battle.StartCoroutine(_battle.RunTriggeredEvents(destination));
        }
    }
}
