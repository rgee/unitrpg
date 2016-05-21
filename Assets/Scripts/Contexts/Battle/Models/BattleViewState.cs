﻿using System;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Signals;
using Contexts.Battle.Utilities;
using Models.Combat;
using Models.Fighting;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using Models.Fighting.Skills;
using Stateless;
using UnityEngine;

namespace Contexts.Battle.Models {
    public class BattleViewState {
        private BattleUIState _state;

        public BattleUIState State {
            get { return _state; }
            set {
                var transition = new StateTransition(_state, value);
                StateTransitionSignal.Dispatch(transition);
                _state = value;
            }
        }

        public int ChapterIndex { get; set; }

        public BattlePhase Phase { get; set; }

        public MovementPath CurrentMovementPath { get; set; }

        public IBattle Battle { get; set; }

        public IMap Map { get; set; }

        public ICombatant SelectedCombatant { get; set; }

        public Vector2 HoveredTile { get; set; }

        public HashSet<CombatActionType> AvailableActions { get; set; }

        public MapDimensions Dimensions { get; set; }

        public void ResetUnitState() {
            CurrentMovementPath = null;
            SelectedCombatant = null;
            AvailableActions = null;
            FightForecast = null;
            SelectedTarget = null;
        }

        public ICombatant SelectedTarget { get; set; }

        public FightForecast FightForecast { get; set; }

        [Inject]
        public StateTransitionSignal StateTransitionSignal { get; set; }


        public BattleViewState() {
            var combatStateMachine = new StateMachine<CombatState, CombatStateTriggers>(CombatState.Start);

            combatStateMachine.Configure(CombatState.Start)
                .Permit(CombatStateTriggers.BattleStarted, CombatState.PlayerTurn);

            combatStateMachine.Configure(CombatState.PlayerTurn)
                .Permit(CombatStateTriggers.ObjectiveFailed, CombatState.Lost)
                .Permit(CombatStateTriggers.AllObjectivesCompleted, CombatState.Won)
                .Permit(CombatStateTriggers.ActionsExhausted, CombatState.EnemyTurn);

            combatStateMachine.Configure(CombatState.EnemyTurn)
                .Permit(CombatStateTriggers.ObjectiveFailed, CombatState.Lost)
                .Permit(CombatStateTriggers.AllObjectivesCompleted, CombatState.Won)
                .Permit(CombatStateTriggers.ActionsExhausted, CombatState.OtherTurn);
            

            combatStateMachine.Configure(CombatState.OtherTurn)
                .Permit(CombatStateTriggers.ObjectiveFailed, CombatState.Lost)
                .Permit(CombatStateTriggers.AllObjectivesCompleted, CombatState.Won)
                .Permit(CombatStateTriggers.ActionsExhausted, CombatState.PlayerTurn);


            var uiStateMachine = new StateMachine<CombatUIState, CombatUITriggers>(CombatUIState.Default);

            uiStateMachine.Configure(CombatUIState.Default)
                .Permit(CombatUITriggers.PhaseChange, CombatUIState.PhaseChanging)
                .Permit(CombatUITriggers.UnitSelected, CombatUIState.ActionMenu);

            uiStateMachine.Configure(CombatUIState.PhaseChanging)
                .Permit(CombatUITriggers.PlayerPhaseStarted, CombatUIState.Default)
                .Permit(CombatUITriggers.NpcPhaseStarted, CombatUIState.NpcLocked);

            uiStateMachine.Configure(CombatUIState.NpcLocked)
                .Permit(CombatUITriggers.PhaseChange, CombatUIState.PhaseChanging);

            uiStateMachine.Configure(CombatUIState.ActionMenu)
                .Permit(CombatUITriggers.MoveSelected, CombatUIState.MoveRange)
                .Permit(CombatUITriggers.AttackSelected, CombatUIState.AttackRange)
                .Permit(CombatUITriggers.Back, CombatUIState.Default);

            uiStateMachine.Configure(CombatUIState.MoveRange)
                .Permit(CombatUITriggers.TargetSelected, CombatUIState.Moving)
                .Permit(CombatUITriggers.Back, CombatUIState.ActionMenu);

            uiStateMachine.Configure(CombatUIState.Moving)
                .Permit(CombatUITriggers.MovementComplete, CombatUIState.Default);

            uiStateMachine.Configure(CombatUIState.AttackRange)
                .Permit(CombatUITriggers.TargetSelected, CombatUIState.CombatForecast)
                .Permit(CombatUITriggers.Back, CombatUIState.ActionMenu);

            uiStateMachine.Configure(CombatUIState.CombatForecast)
                .Permit(CombatUITriggers.ForecastAccepted, CombatUIState.Fighting)
                .Permit(CombatUITriggers.ForecastRejected, CombatUIState.AttackRange)
                .Permit(CombatUITriggers.Back, CombatUIState.AttackRange);

            uiStateMachine.Configure(CombatUIState.Fighting)
                .Permit(CombatUITriggers.FightComplete, CombatUIState.Default);
        }


        public BattlePhase SelectNextPhase() {
            
            var phaseOrder = new List<BattlePhase> {BattlePhase.Player, BattlePhase.Enemy, BattlePhase.Other};
            var nextPhase = Phase;
            var hasUnits = false;
            while (!hasUnits) {
                nextPhase = phaseOrder[(phaseOrder.IndexOf(nextPhase) + 1)%phaseOrder.Count];

                if (nextPhase == Phase) {
                    return nextPhase;
                }

                hasUnits = Battle.GetAliveByArmy(GetArmyType(nextPhase)).Any();
            }

            return nextPhase;
        }

        private static ArmyType GetArmyType(BattlePhase phase) {
            switch (phase) {
                case BattlePhase.Player:
                    return ArmyType.Friendly;
                case BattlePhase.Enemy:
                    return ArmyType.Enemy;
                case BattlePhase.Other:
                    return ArmyType.Other;
                default:
                    throw new ArgumentOutOfRangeException("phase", phase, null);
            }
        }
    }
}