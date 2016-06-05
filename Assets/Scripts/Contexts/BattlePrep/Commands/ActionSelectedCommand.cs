using System;
using Contexts.Battle.Signals;
using Contexts.Battle.Signals.Public;
using Contexts.BattlePrep.Models;
using Contexts.BattlePrep.Signals;
using Contexts.Global.Signals;
using strange.extensions.command.impl;
using strange.extensions.context.api;
using UnityEngine;

namespace Contexts.BattlePrep.Commands {
    public class ActionSelectedCommand : Command {
        [Inject]
        public BattlePrepAction PrepAction { get; set; }

        [Inject]
        public BattleStartSignal BattleStartSignal { get; set; }

        [Inject]
        public BeginSurveyingSignal BeginSurveyingSignal { get; set; }

        [Inject]
        public PushSceneSignal PushSceneSignal { get; set; }

        public override void Execute() {
            switch (PrepAction) {
                case BattlePrepAction.Trade:
                    break;
                case BattlePrepAction.Save:
                    PushSceneSignal.Dispatch("SaveGame");
                    break;
                case BattlePrepAction.Survey:
                    BeginSurveyingSignal.Dispatch();
                    break;
                case BattlePrepAction.Personnel:
                    break;
                case BattlePrepAction.Morale:
                    break;
                case BattlePrepAction.Fight:
                    BattleStartSignal.Dispatch();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
