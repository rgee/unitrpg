using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Utilities;
using DG.Tweening;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using Stateless;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Contexts.Battle.Views {
    public class BubbleMenuView : View {

        [Tooltip("How far from the center the bubbles should be.")]
        public float Scale = 40f;

        public Signal DismissSignal = new Signal();
        public Signal<string> ItemSelectedSignal = new Signal<string>();

        private bool _hidden = true;
        private const float ShowStaggerStepSeconds = 0.1f;
        private const float TransitionDurationSeconds = 0.3f;
        private readonly Vector2 _basisVector = new Vector2(0, 1);
        private readonly Vector3 _buttonScaleFactor = new Vector3(.25f, .25f, .25f);
        private BubbleMenuUtils.MenuStateMachine _stateMachine;
        private HashSet<BubbleMenuItem> _config;

        // Contains a list of degree offsets for each number of concurrently-visible buttons.
        private readonly Dictionary<int, List<float>> _layoutsBySize = new Dictionary<int, List<float>> {
            { 1, new List<float> { 0f } },
            { 2, new List<float> { 40f, -40f } },
            { 3, new List<float> { 0f, 90f, -90f } },
            { 4, new List<float> { 25f, -25f, 90f, -90f } },
            { 5, new List<float> { 0f, 75f, -75f, 135f, -135f } },
        };

        private Stack<string> _path = new Stack<string>();

        private Dictionary<string, GameObject> _bubbles = new Dictionary<string, GameObject>();

        public void Show(HashSet<BubbleMenuItem> config) {
            _hidden = false;
            _config = config;
            _stateMachine = BubbleMenuUtils.CreateStateMachine(config);
            _stateMachine.SelectSignal.AddListener(Select);
            _stateMachine.ChangeLevelSignal.AddListener(ShowLevel);
            _stateMachine.CloseSignal.AddListener(DismissSignal.Dispatch);
            var count = config.Count;
            var copiedConfig = new HashSet<BubbleMenuItem>().Concat(config).ToHashSet();

            copiedConfig.Add(BubbleMenuItem.Leaf("Back", int.MaxValue));
            var configItems = Flatten(copiedConfig);

            foreach (var item in configItems) {
                var buttonName = item.Name;
                var path = item.ResourcePath == null ? "MenuBubbles/" + buttonName : item.ResourcePath;
                var prefab = Resources.Load(path) as GameObject;
                if (prefab == null) {
                    throw new ArgumentException("Could not find menu item at path: " + path);
                }

                var child = Instantiate(prefab);
                child.name = prefab.name;
                child.transform.SetParent(transform);
                child.SetActive(false);

                var eventTrigger = child.GetComponent<EventTrigger>();
                if (eventTrigger == null) {
                    eventTrigger = child.AddComponent<EventTrigger>();
                }

                var entry = new EventTrigger.Entry {eventID = EventTriggerType.PointerClick};

                var nameCopy = buttonName;
                entry.callback.AddListener(eventData => ClickItem(nameCopy));
                eventTrigger.triggers.Add(entry);

                _bubbles[buttonName] = child;
            }
            var positions = GetPoints(count);
            var bubbles = GetGameObjectsForBubbles(copiedConfig);

            ScaleInBubbles(positions, bubbles);
        }

        public void ClickItem(string itemName) {
            _stateMachine.Fire(itemName);
        }

        private IEnumerator FlyToPositions(IEnumerable<Vector3> points, IList<GameObject> bubbles) {
            var seq = DOTween.Sequence();
            var time = 0f;
            var pointList = points.ToList();

            for (var i = 0; i < bubbles.Count; i++) {
                var point = pointList[i];
                var bubble = bubbles[i];
                bubble.SetActive(true);
                bubble.transform.localScale = _buttonScaleFactor;
                seq.Insert(time, bubble.transform.DOLocalMove(point, TransitionDurationSeconds));
                time += ShowStaggerStepSeconds;
            }

            yield return seq.WaitForCompletion();
        }

        private IList<Vector3> GetPoints(int numItems) {
            if (!_layoutsBySize.ContainsKey(numItems)) {
                throw new ArgumentException("Cannot show bubble menu with " + numItems + " items.");
            }
            var rotations = _layoutsBySize[numItems]
                .Concat(new[] {180f});

            return rotations
                .Select(f => Quaternion.Euler(0, 0, f)*(_basisVector*Scale))
                .ToList();
        }

        private void ScaleInBubbles(IList<Vector3> positions, IList<GameObject> bubbles) {
            var transforms = positions.Select((position, i) => {
                var bubble = bubbles[i];
                bubble.transform.localScale = Vector3.zero;
                bubble.transform.localPosition = position;
                return bubble.transform;
            }).ToList();

            var groups = transforms
                .GroupBy(bubble => bubble.transform.localPosition.y)
                .OrderBy(group => group.Key);
            StartCoroutine(ShowBubbleGroups(groups));
        }

        IEnumerator ShowBubbleGroups(IEnumerable<IGrouping<float, Transform>> groups) {
            var enumerable = groups as IList<IGrouping<float, Transform>> ?? groups.ToList();
            foreach (var group in enumerable) {
                foreach (var bubble in group) {
                    bubble.gameObject.SetActive(true);
                }
            }
            yield return StartCoroutine(ScaleBubbleGroup(enumerable, _buttonScaleFactor));
        }

        IEnumerator ScaleBubbleGroup(IEnumerable<IGrouping<float, Transform>> groups, Vector3 scale) {
            var sequence = DOTween.Sequence();
            var time = 0f;
            foreach (var group in groups) {
                foreach (var bubble in group) {
                    sequence.Insert(time, bubble.DOScale(scale, TransitionDurationSeconds));
                }
                time += ShowStaggerStepSeconds;
            }

            yield return sequence.WaitForCompletion();
        }

        private List<GameObject> GetGameObjectsForBubbles(HashSet<BubbleMenuItem> items) {
            return items
                .OrderBy(item => item.Weight)
                .Select(item => {
                    if (!_bubbles.ContainsKey(item.Name)) {
                        throw new ArgumentException("Could not find game object for bubble named: " + item.Name);
                    }

                    return _bubbles[item.Name];
                })
                .ToList();
        }

        private IList<BubbleMenuItem> Flatten(HashSet<BubbleMenuItem> items) {
            return Flatten(new List<BubbleMenuItem>(), items);
        }

        private IList<BubbleMenuItem> Flatten(IList<BubbleMenuItem> list, HashSet<BubbleMenuItem> items) {
            foreach (var item in items) {
                list = Flatten(list, item);
            }

            return list;
        }

        private IList<BubbleMenuItem> Flatten(IList<BubbleMenuItem> list, BubbleMenuItem item) {
            list.Add(item);
            if (item.IsLeaf()) {
                return list;
            }

            return Flatten(list, item.Children);
        }

        public void Hide() {
            if (_hidden) {
                return;
            }

            _hidden = true;
            StopAllCoroutines();
            StartCoroutine(AnimateHide());
        }

        private IEnumerator AnimateHide() {
            var bubbles = _bubbles.Values.Where(bubble => bubble.activeSelf).Select(bubble => bubble.transform);
            var bubbleGroups = bubbles.GroupBy(bubble => bubble.localPosition.y)
                .OrderByDescending(group => group.Key)
                .ToList();
            yield return StartCoroutine(HideBubbleGroups(bubbleGroups));

            _bubbles = new Dictionary<string, GameObject>();
            _stateMachine.SelectSignal.RemoveListener(Select);
            _stateMachine.ChangeLevelSignal.RemoveListener(ShowLevel);
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }

        private BubbleMenuItem FindItemByName(HashSet<BubbleMenuItem> items, string name) {
            foreach (var root in items) {
                if (root.Name == name) {
                    return root;
                } else {
                    var recursiveResult = FindItemByName(root.Children, name);
                    if (recursiveResult != null) {
                        return recursiveResult;
                    }
                }
            }

            return null;
        }

        public IEnumerator HideActiveExceptBack() {
            var bubbles =
                _bubbles.Values
                .Where(bubble => bubble.activeSelf && bubble.name != "Back")
                .Select(bubble => bubble.transform);

            var bubbleGroups = bubbles.GroupBy(bubble => bubble.localPosition.y)
                .OrderByDescending(group => group.Key)
                .ToList();

            yield return StartCoroutine(HideBubbleGroups(bubbleGroups));
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

        private IEnumerator FlyToPosition(Vector3 point, List<GameObject> bubbles) {
            var seq = DOTween.Sequence();
            var time = 0f;
            bubbles.ForEach(bubble => {
                seq.Insert(time, bubble.transform.DOLocalMove(point, TransitionDurationSeconds));
                time += ShowStaggerStepSeconds;
            });

            yield return seq.WaitForCompletion();
            foreach (var bubble in bubbles) {
                bubble.SetActive(false);
            }
        }

        private void ShowLevel(StateMachine<string, string>.Transition transition) {
            var level = transition.Destination;
            if (_path.Count > 0 && transition.Source == _path.Peek()) {

                var lastParent = _path.Pop();
                var parent = FindItemByName(_config, lastParent);
                var isTopLevel = _path.Count <= 0;
                var topLevelBubbles = GetGameObjectsForBubbles(isTopLevel ? _config : parent.Children);
                var activeBubblesExceptBack = _bubbles.Values
                    .Where(bubble => bubble.activeSelf && bubble.name != "Back")
                    .ToList();

                var rootPosition = _bubbles[lastParent].transform.localPosition;
                var bubbleGroups = topLevelBubbles
                    .Select(bubble => bubble.transform)
                    .GroupBy(t => t.localPosition.y)
                    .OrderBy(group => group.Key)
                    .ToList();

                StartCoroutine(ShowBubbleGroups(bubbleGroups));
                StartCoroutine(FlyToPosition(rootPosition, activeBubblesExceptBack));
            } else {
                var root = FindItemByName(_config, level);
                var items = root != null ? root.Children : _config;

                var points = GetPoints(items.Count);

                var itemObject = _bubbles[level];
                var sinkPosition = itemObject.transform.localPosition;

                var sortedBubbles = GetGameObjectsForBubbles(items)
                    .Where(bubble => bubble.name != "Back")
                    .ToList();

                // Set up all the bubbles at the same point as the selected one
                sortedBubbles.ForEach(bubble => bubble.transform.localPosition = sinkPosition);

                StartCoroutine(HideActiveExceptBack());
                StartCoroutine(FlyToPositions(points, sortedBubbles));
                _path.Push(level);
            }
        }

        private void Select(string item) {
            // Just relay up
            ItemSelectedSignal.Dispatch(item);
        }
    }
}