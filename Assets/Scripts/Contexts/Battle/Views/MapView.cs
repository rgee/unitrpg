﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Contexts.Battle.Utilities;
using Models.Fighting.Battle;
using Models.Fighting.Execution;
using Models.Fighting.Maps;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;
using Utils;

namespace Contexts.Battle.Views {
    public class MapView : View {
        public const float FIGHT_PHASE_WAIT_SECONDS = 0.4f;
        public Signal<Vector2> MapClicked = new Signal<Vector2>();
        public Signal<Vector2> MapRightClicked = new Signal<Vector2>();
        public Signal<Vector2> MapHovered = new Signal<Vector2>(); 
        public Signal MoveComplete = new Signal();
        public Signal FightComplete = new Signal();
        public string MapId;
        public int ChapterNumber;
        public int Width;
        public int TileSize;
        public int Height;

#region Fields exposed so sequences can hack around scene initialization
        public IMap Map;
        public ICombatantDatabase CombatantDatabase;
#endregion

        private FightPhaseAnimator _phaseAnimator;

        void Awake() {
            _phaseAnimator = GetComponent<FightPhaseAnimator>();
        }

        void Update() {
            var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            var dimensions = GetDimensions();
            var mouseGridPosition = dimensions.GetGridPositionForWorldPosition(mousePosition);
            if (Input.GetMouseButtonDown(0)) {
                MapClicked.Dispatch(mouseGridPosition);
            } else if (Input.GetMouseButtonDown(1)) { 
                MapRightClicked.Dispatch(mouseGridPosition);
            } else {
                MapHovered.Dispatch(mouseGridPosition);
            }
        }

        public IEnumerator MoveUnit(string id, List<Vector2> path) {
            var combatant = FindCombatantViewById(id);
            var dimensoins = GetDimensions(); 
            var worldPositions = path.Select(pos => dimensoins.GetWorldPositionForGridPosition(pos)).ToList();
            yield return StartCoroutine(DoMove(worldPositions, combatant));
        }

        private CombatantView FindCombatantViewById(string id) {
            var unitContainer = transform.Find("Units");

            return (from Transform unit in unitContainer select unit.GetComponent<CombatantView>())
                .FirstOrDefault(combatantComponent => combatantComponent.CombatantId == id);
        }

        public MapDimensions GetDimensions() {
            return new MapDimensions(Width, Height, TileSize);
        }

        private IEnumerator DoMove(IList<Vector3> positions, CombatantView combatant) {
            var dimensions = GetDimensions();
            yield return StartCoroutine(combatant.FollowPath(positions, dimensions));
            MoveComplete.Dispatch();
        }

        public List<CombatantDatabase.CombatantReference> GetCombatants() {
            var unitContainer = transform.Find("Units").gameObject;
            var units = unitContainer.GetComponentsInChildren<CombatantView>();
            var dimensions = GetDimensions();

            return units.Select(unit => {
                return new CombatantDatabase.CombatantReference {
                    CharacterId = unit.CharacterId,
                    Id = unit.CombatantId,
                    Position = dimensions.GetGridPositionForWorldPosition(unit.transform.position),
                    Name = unit.CharacterName,
                    Army = unit.Army
                };
            }).ToList();
        }

        public IEnumerator AnimateFight(FinalizedFight fight) {
            var phases = new List<FightPhase> {fight.InitialPhase};

            if (fight.FlankerPhase != null) {
                phases.Add(fight.FlankerPhase);
            }

            if (fight.CounterPhase != null) {
                phases.Add(fight.CounterPhase);
            }

            if (fight.DoubleAttackPhase != null) {
                phases.Add(fight.DoubleAttackPhase);
            }

            var participants = new List<CombatantView>();

            var primaryAttacker = FindCombatantViewById(fight.InitialPhase.Initiator.Id);
            var primaryAttackerDirection = MathUtils.DirectionTo(fight.InitialPhase.Initiator.Position,
                fight.InitialPhase.Receiver.Position);
            primaryAttacker.PrepareForCombat(primaryAttackerDirection);
            participants.Add(primaryAttacker);

            var defender = FindCombatantViewById(fight.InitialPhase.Receiver.Id);
            var defenderDirection = MathUtils.DirectionTo(fight.InitialPhase.Receiver.Position,
                fight.InitialPhase.Initiator.Position);
            defender.PrepareForCombat(defenderDirection);
            participants.Add(defender);

            if (fight.FlankerPhase != null) {
                var flanker = FindCombatantViewById(fight.FlankerPhase.Initiator.Id);
                var flankerDirection = MathUtils.DirectionTo(fight.FlankerPhase.Initiator.Position,
                    fight.FlankerPhase.Receiver.Position);
                flanker.PrepareForCombat(flankerDirection);
                participants.Add(flanker);
            }

            yield return StartCoroutine(AnimateFights(phases));

            foreach (var combatantView in participants) {
                combatantView.ReturnToRest();
            }
        }

        private IEnumerator AnimateFights(IEnumerable<FightPhase> phases) {
            foreach (var phase in phases) {
                yield return StartCoroutine(AnimateFightPhase(phase));
                yield return new WaitForSeconds(FIGHT_PHASE_WAIT_SECONDS);
            }
        }

        private IEnumerator AnimateFightPhase(FightPhase phase) {
            var initiator = FindCombatantViewById(phase.Initiator.Id);
            var receiver = FindCombatantViewById(phase.Receiver.Id);

            yield return _phaseAnimator.Animate(phase, initiator, receiver, GetDimensions());
        }

        void OnDrawGizmos() {
            // Draw a green outline around the map.
            Gizmos.color = Color.green;
            var totalWidth = Width*TileSize;
            var totalHeight = Height*TileSize;

            var offset = new Vector3(-TileSize / 2.0f, -TileSize / 2.0f);

            var tlCorner = transform.position + offset +new Vector3(0, totalHeight);
            var trCorner = transform.position + offset + new Vector3(totalWidth, totalHeight);
            var blCorner = transform.position + offset;
            var brCorner = transform.position + offset + new Vector3(totalWidth, 0);

            Gizmos.DrawLine(tlCorner, trCorner);
            Gizmos.DrawLine(trCorner, brCorner);
            Gizmos.DrawLine(brCorner, blCorner);
            Gizmos.DrawLine(blCorner, tlCorner);
        }
    }
}