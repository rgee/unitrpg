using System;
using System.Collections.Generic;
using System.Linq;
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
                bubble.transform.localScale = new Vector3(.5f, .5f, .5f);
                bubble.transform.localPosition = position;

                return bubble.transform;
            }).ToList();
        }

        IEnumerable<Vector3> _getPoints(int numActions) {
            var rotations = _layoutsBySize[numActions];
            return rotations.Select((f => Quaternion.Euler(0, 0, f)*(_basisVector*Scale)));
        } 

        void Update() {
            if (_bubbles.Count <= 0) {
                return;
            }

            var points = _getPoints(_bubbles.Count).ToList();
            for (var i = 0; i < points.Count; i++) {
                _bubbles[i].transform.localPosition = points[i];
            }
        }

        public CombatAction? SelectedAction { get; set; }

        public void Hide() {
            _bubbles.ForEach(Destroy);
        }
    }
}
