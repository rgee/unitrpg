using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Combat;
using Contexts.Battle.Utilities;
using Models.Fighting.Battle;
using Models.Fighting.Characters;
using Models.Fighting.Execution;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapView : View {
        public Signal<Vector2> MapClicked = new Signal<Vector2>();
        public Signal<Vector2> MapHovered = new Signal<Vector2>(); 
        public Signal MoveComplete = new Signal();
        public Signal FightComplete = new Signal();
        public int Width;
        public int TileSize;
        public int Height;

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
            } else {
                MapHovered.Dispatch(mouseGridPosition);
            }
        }

        public void MoveUnit(string id, List<Vector2> path) {
            var combatant = FindCombatantViewById(id);
            var dimensoins = GetDimensions(); 
            var worldPositions = path.Skip(1).Select(pos => dimensoins.GetWorldPositionForGridPosition(pos)).ToList();
            StartCoroutine(DoMove(worldPositions, combatant));
        }

        private CombatantView FindCombatantViewById(string id) {
            var unitContainer = transform.FindChild("Units");

            foreach (Transform unit in unitContainer) {
                var combatantComponent = unit.GetComponent<CombatantView>();
                if (combatantComponent.CombatantId == id) {
                    return combatantComponent;
                }
            }

            return null;
        }

        public MapDimensions GetDimensions() {
            return new MapDimensions(Width, Height, TileSize);
        }

        private IEnumerator DoMove(IList<Vector3> positions, CombatantView combatant) {
            var dimensions = GetDimensions();
            yield return StartCoroutine(combatant.FollowPath(positions, dimensions));
            MoveComplete.Dispatch();
        } 

        private GameObject FindUnitById(string id) {
            var unitContainer = transform.FindChild("Units");

            foreach (Transform unit in unitContainer) {
                var unitComponent = unit.GetComponent<Grid.Unit>();
                if (unitComponent.Id == id) {
                    return unit.gameObject;
                }
            }

            return null;
        }

        public List<CombatantDatabase.CombatantReference> GetCombatants() {
            var unitContainer = transform.FindChild("Units").gameObject;
            var units = unitContainer.GetComponentsInChildren<Grid.Unit>();

            return units.Select(unit => {
                var character = unit.GetCharacter();
                return new CombatantDatabase.CombatantReference {
                    Id = unit.Id,
                    Position = unit.gridPosition,
                    Name = character.Name,

                    // TODO: Have dropdown for army type
                    Army = unit.friendly ? ArmyType.Friendly : ArmyType.Enemy
                };
            }).ToList();
        }

        public List<Vector2> GetObstructedPositions() {
            var obstructions = transform.FindChild("Obstructions");
            var results = new List<Vector2>();
            var dimensions = GetDimensions();

            foreach (Transform obstacle in obstructions) {
                var gridPosition = dimensions.GetGridPositionForWorldPosition(obstacle.position);
                results.Add(gridPosition);
            }
            
            return results;
        }

        public void AnimateFight(FinalizedFight fight) {
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

            var participants = new List<Grid.Unit>();

            var primaryAttacker = FindUnitById(fight.InitialPhase.Initiator.Id).GetComponent<Grid.Unit>();
            primaryAttacker.InCombat = true;
            participants.Add(primaryAttacker);

            var defender = FindUnitById(fight.InitialPhase.Receiver.Id).GetComponent<Grid.Unit>();
            defender.InCombat = true;
            participants.Add(defender);

            primaryAttacker.Facing = MathUtils.DirectionTo(fight.InitialPhase.Initiator.Position,
                fight.InitialPhase.Receiver.Position);
            defender.Facing = MathUtils.DirectionTo(fight.InitialPhase.Receiver.Position,
                fight.InitialPhase.Initiator.Position);

            if (fight.FlankerPhase != null) {
                var flanker = FindUnitById(fight.FlankerPhase.Initiator.Id).GetComponent<Grid.Unit>();
                flanker.InCombat = true;
                flanker.Facing = MathUtils.DirectionTo(fight.FlankerPhase.Initiator.Position,
                    fight.FlankerPhase.Receiver.Position);
                participants.Add(flanker);
            }

            FightComplete.AddOnce(() => {
                foreach (var unit in participants) {
                    unit.ReturnToRest();
                }
            });

            StartCoroutine(AnimateFights(phases));
        }

        private IEnumerator AnimateFights(IEnumerable<FightPhase> phases) {
            foreach (var phase in phases) {
                yield return StartCoroutine(AnimateFightPhase(phase));
                yield return new WaitForSeconds(0.4f);
            }

            FightComplete.Dispatch();
        }

        private IEnumerator AnimateFightPhase(FightPhase phase) {
            Debug.LogFormat("Animating {0} vs {1}", phase.Initiator.Name, phase.Receiver.Name);
            var initiator = FindUnitById(phase.Initiator.Id).GetComponent<Grid.Unit>();
            var receiver = FindUnitById(phase.Receiver.Id).GetComponent<Grid.Unit>();

            yield return _phaseAnimator.Animate(phase, initiator, receiver);
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