using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Models.Combat;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class ActionMenuView : View {
        [Tooltip("How far from the center the bubbles should be.")]
        public float Scale = 10f;

        [Tooltip("The prefab for each bubble.")]
        public GameObject BubblePrefab;

        /// <summary>
        /// Dispatched the user clicks a bubble for an action.
        /// </summary>
        public Signal<CombatActionType> ActionTypeSelectedSignal = new Signal<CombatActionType>(); 

        /// <summary>
        /// Dispatched when the user clicks the top level cancelation button.
        /// </summary>
        public Signal CancelSignal = new Signal();

        /// <summary>
        /// Dispatched when the user clicks the Fight menu back button.
        /// </summary>
        public Signal BackSignal = new Signal();

        /// <summary>
        /// Dispatched when the user clicks to enter the fight menu.
        /// </summary>
        public Signal FightSignal = new Signal();

        private Transform _containerTransform;
        private readonly Vector3 _buttonScale = new Vector3(.25f, .25f, .25f);
        private readonly Vector2 _basisVector = new Vector2(0, 1);
        private readonly float _showStaggerStepSeconds = 0.1f;
        private readonly float _transitionDurationSeconds =  0.3f;

        private bool _inFightMenu;
        private bool _hidden = true;

        private readonly Dictionary<int, List<float>> _layoutsBySize = new Dictionary<int, List<float>> {
            { 1, new List<float> { 0f } },
            { 2, new List<float> { 40f, -40f } },
            { 3, new List<float> { 0f, 90f, -90f } },
            { 4, new List<float> { 25f, -25f, 90f, -90f } },
            { 5, new List<float> { 0f, 75f, -75f, 135f, -135f } }
        };

        private readonly Dictionary<string, GameObject> _bubblesByActionName = new Dictionary<string, GameObject>();

        private IEnumerable<CombatActionType> _currentFightActions;
        private IEnumerable<CombatActionType> _currentActions;

        private readonly List<MenuAction> _actions = new List<MenuAction> {
            new MenuAction("Back", int.MinValue),
            new MenuAction("Fight", 8),
            new MenuAction("Move", 7),
            new MenuAction("Item", 6),
            new MenuAction("Trade", 5),
            new MenuAction("Talk", 4),
            new MenuAction("Attack", 3),
            new MenuAction("Brace", 2),
            new MenuAction("Cover", 1),
            new MenuAction("Wait", 0),
            new MenuAction("Use", 9)
        };

        private struct MenuAction {
            public readonly string Name;
            public readonly int Priority;

            public MenuAction(string name, int priority) {
                Name = name;
                Priority = priority;
            }
        }

        protected override void Awake() {
            base.Awake();
            _containerTransform = GameObject.Find("Container").transform;
            foreach (var action in _actions) {
                var button =_containerTransform.Find(action.Name);
                if (button != null) {
                    _bubblesByActionName[action.Name] = button.gameObject;
                }
            }
        }


        public void SelectAction(string action) {
            switch (action) {
                case "Back":
                    if (_inFightMenu) {
                        _inFightMenu = false;
                        BackSignal.Dispatch();
                    } else {
                        CancelSignal.Dispatch();
                    }
                    break;
                case "Fight":
                    _inFightMenu = true;
                    FightSignal.Dispatch();
                    break;
                default:
                    var actionEnum = (CombatActionType)Enum.Parse(typeof(CombatActionType), action);
                    ActionTypeSelectedSignal.Dispatch(actionEnum);
                    break;
            }
        }

        public void Show(IEnumerable<CombatActionType> actions, IEnumerable<CombatActionType> fightActions) {
            var combatActionTypes = actions as IList<CombatActionType> ?? actions.ToList();
            _currentActions = combatActionTypes;
            _currentFightActions = fightActions;

            var combatActions = actions as IList<CombatActionType> ?? combatActionTypes.ToList();

            var sortedBubbles = GetSortedBubblesForActions(combatActions);

            // Assign the points to each bubble iterating through the parallel arrays
            var numActions = combatActions.Count;
            var points = _getPoints(numActions);

            // Deactivate all bubbles and only activate the ones we intend to show
            foreach (var bubble in _bubblesByActionName.Values) {
                bubble.SetActive(false);
            }
            ScaleInBubbles(points, sortedBubbles);
        }

        private List<GameObject> GetSortedBubblesForActions(IEnumerable<CombatActionType> combatActions) {
            // Convert the combat action enums to MenuAction names
            var actionStrings = combatActions.Select(action => action.ToString()).ToHashSet();

            // Get the MenuActions being requested and sort them by priroity in descending order
            var relevantMenuActions = _actions
                .Where(action => actionStrings.Contains(action.Name))
                .Concat(new[] {_actions.Find(action => action.Name == "Back")})
                .OrderByDescending(action => action.Priority);

            // Get the needed bubble game objects in the same order
            var sortedBubbles = relevantMenuActions.Select(action => _bubblesByActionName[action.Name]).ToList();
            return sortedBubbles;
        }

        public void ReturnToTop() {
            var topLevelBubbles = GetSortedBubblesForActions(_currentActions.ToList());
            var activeBubblesExceptBack = _bubblesByActionName.Values
                .Where(bubble => bubble.activeSelf && bubble.name != "Back")
                .ToList();
            var fightPosition = _bubblesByActionName["Fight"].transform.localPosition;

            var bubbleGroups = topLevelBubbles
                .Select(bubble => bubble.transform)
                .GroupBy(t => t.localPosition.y)
                .OrderBy(group => group.Key)
                .ToList();

            StartCoroutine(ShowBubbleGroups(bubbleGroups));
            StartCoroutine(FlyToPosition(fightPosition, activeBubblesExceptBack));

        }

        private IEnumerator FlyToPosition(Vector3 point, List<GameObject> bubbles) {
            var seq = DOTween.Sequence();
            var time = 0f;
            bubbles.ForEach(bubble => {
                seq.Insert(time, bubble.transform.DOLocalMove(point, _transitionDurationSeconds));
                time += _showStaggerStepSeconds;
            });

            yield return seq.WaitForCompletion();
            foreach (var bubble in bubbles) {
                bubble.SetActive(false);
            }
        }

        public void ShowFightSubMenu() {
            _hidden = false;
            var points = _getPoints(_currentFightActions.Count());

            var fightPosition = _bubblesByActionName["Fight"].transform.localPosition;
            var sortedBubbles = GetSortedBubblesForActions(_currentFightActions.ToList())
                .Where(bubble => bubble.name != "Back")
                .ToList();

            // Set up all the bubbles at the same point as the selected one
            sortedBubbles.ForEach(bubble => bubble.transform.localPosition = fightPosition);

            StartCoroutine(HideActiveExceptBack());
            StartCoroutine(FlyToPositions(points, sortedBubbles));
        }


        private IEnumerator FlyToPositions(IEnumerable<Vector3> points, IList<GameObject> bubbles) {
            var seq = DOTween.Sequence();
            var time = 0f;
            var pointList = points.ToList();

            for (var i = 0; i < bubbles.Count; i++) {
                var point = pointList[i];
                var bubble = bubbles[i];
                bubble.SetActive(true);
                bubble.transform.localScale = _buttonScale;
                seq.Insert(time, bubble.transform.DOLocalMove(point, _transitionDurationSeconds));
                time += _showStaggerStepSeconds;
            }

            yield return seq.WaitForCompletion();
        }

        private void ScaleInBubbles(IEnumerable<Vector3> points, IList<GameObject> sortedBubbles) {
            var bubbles = points.Select((position, i) => {
                var bubble = sortedBubbles[i];
                bubble.transform.localScale = Vector3.zero;
                bubble.transform.localPosition = position;

                return bubble.transform;
            }).ToList();


            var bubbleGroups = bubbles.GroupBy(bubble => bubble.transform.localPosition.y)
                .OrderBy(group => group.Key);
            StartCoroutine(ShowBubbleGroups(bubbleGroups));
        }

        public IEnumerator HideActiveExceptBack() {
            var bubbles =
                _bubblesByActionName.Values
                .Where(bubble => bubble.activeSelf && bubble.name != "Back")
                .Select(bubble => bubble.transform);
            var bubbleGroups = bubbles.GroupBy(bubble => bubble.localPosition.y)
                .OrderByDescending(group => group.Key)
                .ToList();

            yield return StartCoroutine(HideBubbleGroups(bubbleGroups));
        }


        IEnumerator ShowBubbleGroups(IEnumerable<IGrouping<float, Transform>> groups) {
            var enumerable = groups as IList<IGrouping<float, Transform>> ?? groups.ToList();
            foreach (var group in enumerable) {
                foreach (var bubble in group) {
                    bubble.gameObject.SetActive(true);
                }
            }
            yield return StartCoroutine(ScaleBubbleGroup(enumerable, _buttonScale));
        }

        IEnumerator HideBubbleGroups(IEnumerable<IGrouping<float, Transform>> groups) {
            var enumerable = groups as IList<IGrouping<float, Transform>> ?? groups.ToList();
            yield return StartCoroutine(ScaleBubbleGroup(enumerable, Vector3.zero));
            foreach (var group in enumerable) {
                foreach (var bubble in group) {
                    bubble.gameObject.SetActive(false);
                }
            }
        }

        IEnumerator ScaleBubbleGroup(IEnumerable<IGrouping<float, Transform>> groups, Vector3 scale) {
            var sequence = DOTween.Sequence();
            var time = 0f;
            foreach (var group in groups) {
                foreach (var bubble in group) {
                    sequence.Insert(time, bubble.DOScale(scale, _transitionDurationSeconds));
                }
                time += _showStaggerStepSeconds;
            }

            yield return sequence.WaitForCompletion();
        }
        
        IEnumerable<Vector3> _getPoints(int numActions) {
            var rotations = _layoutsBySize[numActions]
                .Concat(new[] {180f});

            return rotations.Select((f => Quaternion.Euler(0, 0, f)*(_basisVector*Scale)));
        } 


        public IEnumerator HideAnimated() {
            var bubbles =
                _bubblesByActionName.Values.Where(bubble => bubble.activeSelf).Select(bubble => bubble.transform);
            var bubbleGroups = bubbles.GroupBy(bubble => bubble.localPosition.y)
                .OrderByDescending(group => group.Key)
                .ToList();
            yield return StartCoroutine(HideBubbleGroups(bubbleGroups));
        }

        public void Show(Vector3 position, HashSet<CombatActionType> actions) {
            if (!_hidden) {
                return;
            }

            var fightActions = new List<CombatActionType> {
                CombatActionType.Attack,
                CombatActionType.Brace
            };

            _hidden = false;
            transform.position = position;

            Show(actions, fightActions);
        }

        public void Hide() {
            if (_hidden) {
                return;
            }

            _hidden = true;
            StartCoroutine(HideAnimated());
        }
    }
}