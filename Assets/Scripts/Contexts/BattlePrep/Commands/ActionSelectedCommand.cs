using System;
using Contexts.Battle.Signals;
using Contexts.BattlePrep.Models;
using Contexts.BattlePrep.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.BattlePrep.Commands {
    public class ActionSelectedCommand : Command {
        [Inject]
        public BattlePrepAction PrepAction { get; set; }

        [Inject]
        public ClosePrepSignal ClosePrepSignal { get; set; }

        public override void Execute() {
            switch (PrepAction) {
                case BattlePrepAction.Trade:
                    break;
                case BattlePrepAction.Survey:
                    break;
                case BattlePrepAction.Personnel:
                    break;
                case BattlePrepAction.Morale:
                    break;
                case BattlePrepAction.Fight:
                    ClosePrepSignal.Dispatch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
