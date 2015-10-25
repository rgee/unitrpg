using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Models.Combat;
using UnityEngine;

namespace UI.ActionMenu.Bubbles {
    public class BubbleActionMenu : MonoBehaviour, IActionMenuView {
        [Tooltip("How far from the center the bubbles should be.")]
        public float Scale = 10f;

        [Tooltip("The prefab for each bubble.")]
        public GameObject BubblePrefab;

        private Transform _containerTransform;
        private List<Transform> _bubbles = new List<Transform>(); 

        private readonly Vector2 _basisVector = new Vector2(0, 1);
        private readonly float _showStaggerStepSeconds = 0.1f;
        private readonly float _transitionDurationSeconds =  0.3f;

        private readonly Dictionary<int, List<float>> _layoutsBySize = new Dictionary<int, List<float>> {
            { 1, new List<float> { 0f } },
            { 2, new List<float> { -20f, 20f } },
            { 3, new List<float> { 0f, -90f, 90f } },
            { 4, new List<float> { -25f, 25f, -90f, 90f } },
            { 5, new List<float> { 0f, -75f, 75f, -135f, 135f } }
        };

        void Awake() {
            _containerTransform = GameObject.Find("Container").transform;
        }

        public void Show(IEnumerable<CombatAction> actions) {
            var numActions = actions.Count();
            _bubbles = _getPoints(numActions).Select((position) => {
                var bubble = Instantiate(BubblePrefab);
                bubble.transform.SetParent(_containerTransform);
                bubble.transform.localScale = Vector3.zero;
                bubble.transform.localPosition = position;

                return bubble.transform;
            }).ToList();


            var bubbleGroups = _bubbles.GroupBy(bubble => bubble.transform.localPosition.y)
                .OrderBy(group => group.Key);
            StartCoroutine(ShowBubbleGroups(bubbleGroups));
        }

        IEnumerator ShowBubbleGroups(IEnumerable<IGrouping<float, Transform>> groups) {
            yield return StartCoroutine(ScaleBubbleGroup(groups, new Vector3(.5f, .5f, .5f)));
        }

        IEnumerator HideBubbleGroups(IEnumerable<IGrouping<float, Transform>> groups) {
            yield return StartCoroutine(ScaleBubbleGroup(groups, Vector3.zero));
            foreach (var bubbleTransform in _bubbles) {
                Destroy(bubbleTransform.gameObject);
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
            var rotations = _layoutsBySize[numActions];
            return rotations.Select((f => Quaternion.Euler(0, 0, f)*(_basisVector*Scale)));
        } 

        public CombatAction? SelectedAction { get; set; }

        public void Hide() {
            var bubbleGroups = _bubbles.GroupBy(bubble => bubble.transform.localPosition.y)
                .OrderByDescending(group => group.Key);
            StartCoroutine(HideBubbleGroups(bubbleGroups));
        }
    }
}
