using System;
using Contexts.Battle.Models;
using strange.extensions.command.impl;
using UnityEngine;

namespace Contexts.Battle.Commands {
    public class ActionCompleteCommand : Command {
        [Inject]
        public BattleViewState Model { get; set; }

        public override void Execute() {

            if (!Model.Battle.ShouldTurnEnd()) {
                Model.ResetUnitState();
                Model.State = BattleUIState.SelectingUnit;
            } else {
               Debug.Log("TURN SHOULD END"); 
            }
        }
    }
}
