using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu.Bubbles
{
    public class BubbleActionMenu : MonoBehaviour, IActionMenuView {
        [Tooltip("How far from the center the bubbles should be.")]
        public float Scale = 10f;

        [Tooltip("The prefab for each bubble.")]
        public GameObject BubblePrefab;

        private Transform _containerTransform;

        private readonly Vector2 _basisVector = new Vector2(0, 1);
        private readonly float _showStaggerStepSeconds = 0.1f;
        private readonly float _transitionDurationSeconds =  0.3f;

        private readonly Dictionary<string, GameObject> _bubblesByActionName = new Dictionary<string, GameObject>();

        private readonly Dictionary<int, List<float>> _layoutsBySize = new Dictionary<int, List<float>> {
            { 1, new List<float> { 0f } },
            { 2, new List<float> { 20f, -20f } },
            { 3, new List<float> { 0f, 90f, -90f } },
            { 4, new List<float> { 25f, -25f, 90f, -90f } },
            { 5, new List<float> { 0f, 75f, -75f, 135f, -135f } }
        };

        private struct MenuAction {
            public readonly string Name;
            public readonly int Priority;

            public MenuAction(string name, int priority) {
                Name = name;
                Priority = priority;
            }
        }

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
            new MenuAction("Wait", 0)
        }; 

        void Awake() {
            _containerTransform = GameObject.Find("Container").transform;
            foreach (var action in _actions) {
                var button =_containerTransform.Find(action.Name);
                if (button != null) {
                    _bubblesByActionName[action.Name] = button.gameObject;
                }
            }
        }

        public void Show(IEnumerable<CombatAction> actions) {
            var combatActions = actions as IList<CombatAction> ?? actions.ToList();

            // Convert the combat action enums to MenuAction names
            var actionStrings = combatActions.Select(action => action.ToString()).ToHashSet();

            // Get the MenuActions being requested and sort them by priroity in descending order
            var relevantMenuActions = _actions
                .Where(action => actionStrings.Contains(action.Name))
                .Concat(new[] { _actions.Find(action => action.Name == "Back") })
                .OrderByDescending(action => action.Priority);

            // Deactivate all bubbles and only activate the ones we intend to show
            foreach (var bubble in _bubblesByActionName.Values) {
                bubble.SetActive(false);
            }

            // Get the needed bubble game objects in the same order
            var sortedBubbles = relevantMenuActions.Select(action => _bubblesByActionName[action.Name]).ToList();

            // Assign the points to each bubble iterating through the parallel arrays
            var numActions = combatActions.Count();
            var bubbles = _getPoints(numActions).Select((position, i) => {
                var bubble = sortedBubbles[i];
                bubble.SetActive(true);
                bubble.transform.localScale = Vector3.zero;
                bubble.transform.localPosition = position;

                return bubble.transform;
            }).ToList();


            var bubbleGroups = bubbles.GroupBy(bubble => bubble.transform.localPosition.y)
                .OrderBy(group => group.Key);
            StartCoroutine(ShowBubbleGroups(bubbleGroups));
        }

        public void SelectAction(string action) {
            switch (action) {
                case "Back":
                    CombatEventBus.Backs.Dispatch();
                    break;
                case "Fight":

                    break;
                default:
                    var actionEnum = (CombatAction) Enum.Parse(typeof(CombatAction), action);
                    SelectedAction = actionEnum;
                    break;
            }
        }

        IEnumerator ShowBubbleGroups(IEnumerable<IGrouping<float, Transform>> groups) {
            yield return StartCoroutine(ScaleBubbleGroup(groups, new Vector3(.5f, .5f, .5f)));
        }

        IEnumerator HideBubbleGroups(IEnumerable<IGrouping<float, Transform>> groups) {
            yield return StartCoroutine(ScaleBubbleGroup(groups, Vector3.zero));
            foreach (var bubble in _bubblesByActionName.Values) {
                bubble.SetActive(false);
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

        public CombatAction? SelectedAction { get; set; }

        public void Hide() {
            var bubbles =
                _bubblesByActionName.Values.Where(bubble => bubble.activeSelf).Select(bubble => bubble.transform);
            var bubbleGroups = bubbles.GroupBy(bubble => bubble.localPosition.y)
                .OrderByDescending(group => group.Key);
            StartCoroutine(HideBubbleGroups(bubbleGroups));
        }
    }
}
