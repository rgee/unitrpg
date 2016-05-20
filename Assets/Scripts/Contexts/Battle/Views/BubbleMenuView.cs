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

namespace Contexts.Battle.Views {
    public class BubbleMenuView : View {

        [Tooltip("How far from the center the bubbles should be.")]
        public float Scale = 10f;
        public Signal<string> ItemSelectedSignal = new Signal<string>();

        private const float ShowStaggerStepSeconds = 0.1f;
        private const float TransitionDurationSeconds = 0.3f;
        private readonly Vector2 _basisVector = new Vector2(0, 1);
        private readonly Vector3 _buttonScaleFactor = new Vector3(.25f, .25f, .25f);
        private BubbleMenuUtils.MenuStateMachine _stateMachine;
        // Contains a list of degree offsets for each number of concurrently-visible buttons.
        private readonly Dictionary<int, List<float>> _layoutsBySize = new Dictionary<int, List<float>> {
            { 1, new List<float> { 0f } },
            { 2, new List<float> { 40f, -40f } },
            { 3, new List<float> { 0f, 90f, -90f } },
            { 4, new List<float> { 25f, -25f, 90f, -90f } },
            { 5, new List<float> { 0f, 75f, -75f, 135f, -135f } }
        };

        private Dictionary<string, GameObject> _bubbles = new Dictionary<string, GameObject>();

        public void Show(HashSet<BubbleMenuItem> config) {
            _stateMachine = BubbleMenuUtils.CreateStateMachine(config);
            _stateMachine.SelectSignal.AddListener(Select);
            _stateMachine.ChangeLevelSignal.AddListener(ShowLevel);
            var prefabNames = GetAllNames(config);
            config.Add(BubbleMenuItem.Leaf("Back", int.MaxValue));

            foreach (var buttonName in prefabNames) {
                var path = "MenuBubbles/" + buttonName;
                var child = Instantiate(Resources.Load(path)) as GameObject;
                if (child == null) {
                    throw new ArgumentException("Could not find menu item by name " + buttonName);
                }
                child.transform.SetParent(transform);
                child.SetActive(false);
            }
            var positions = GetPoints(config.Count);
            var bubbles = GetGameObjectsForBubbles(config);

            ScaleInBubbles(positions, bubbles);
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
                .Select(item => _bubbles[item.Name])
                .ToList();
        }

        private List<string> GetAllNames(HashSet<BubbleMenuItem> items) {
            return GetAllNames(new List<string>(), items);
        }

        private List<string> GetAllNames(List<string> names, HashSet<BubbleMenuItem> items) {
            foreach (var item in items) {
                names = GetAllNames(names, item);
            }

            return names;
        }

        private List<string> GetAllNames(List<string> names, BubbleMenuItem item) {
            names.Add(item.Name);
            if (item.IsLeaf()) {
                return names;
            }

            return GetAllNames(names, item.Children);
        }

        public void Hide() {
            StartCoroutine(AnimateHide());
        }

        private IEnumerator AnimateHide() {
            // transition closed
            yield return null;

            _bubbles = new Dictionary<string, GameObject>();
            _stateMachine.SelectSignal.RemoveListener(Select);
            _stateMachine.ChangeLevelSignal.RemoveListener(ShowLevel);
            foreach (Transform child in transform) {
                Destroy(child.gameObject);
            }
        }

        private void ShowLevel(string level) {
            // transition between levels
        }

        private void Select(string item) {
            // Just relay up
            ItemSelectedSignal.Dispatch(item);
        }
    }
}