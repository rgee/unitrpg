using System.Collections.Generic;
using System.Linq;
using Contexts.Battle.Models;
using Contexts.Battle.Utilities;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;

namespace Contexts.Battle.Views {
    public class MapHighlightView : View {
        public int BaseSortOrder = -1000;
        public GameObject MapHighlightPrefab;
        public Material GlobalEnemyMaterial;
        public Material AttackSelectionMaterial;
        public Material HoverSelectionMaterial;
        public Material MovementSelectionMaterial;
        public Material SpecificEnemyMaterial;

        private HashSet<GameObject> _highlights;

        private GameObject _hoverHighlight;

        void Awake() {
            base.Awake();
            _hoverHighlight = transform.FindChild("Hover").gameObject;
        }

        public void SetHighlightedPosition(Vector3 position) {
            _hoverHighlight.transform.position = position;
            _hoverHighlight.SetActive(true);
        }

        public void HighlightPositions(IEnumerable<Vector2> positions, HighlightLevel level, MapDimensions dimensions) {
            var worldPositions =
                positions.Select(pos => dimensions.GetWorldPositionForGridPosition(pos)).ToList();

            _highlights = worldPositions.Select(pos => CreateHighlight(pos, level)).ToHashSet();
        }

        public void ClearHighlightedPositions() {
            foreach (var highlight in _highlights) {
                Destroy(highlight);
            }

            _highlights = null;
        }

        public void DisableHoverHighlight() {
            _hoverHighlight.SetActive(false);
        }

        public void EnableHoverHighlight() {
            _hoverHighlight.SetActive(true);
        }

        private GameObject CreateHighlight(Vector3 pos, HighlightLevel level) {
            var highlight = Instantiate(MapHighlightPrefab);
            highlight.transform.position = pos;
            highlight.transform.SetParent(transform);

            var highlightRenderer = highlight.GetComponent<Renderer>();
            highlightRenderer.sortingLayerName = "Default";
            highlightRenderer.sortingOrder = BaseSortOrder + (int)level;

            switch (level) {
                case HighlightLevel.PlayerMove:
                    highlightRenderer.material = MovementSelectionMaterial;
                    break;
                case HighlightLevel.PlayerAttack:
                    highlightRenderer.material = AttackSelectionMaterial;
                    break;
                case HighlightLevel.PlayerHover:
                    highlightRenderer.material = HoverSelectionMaterial;
                    break;
                case HighlightLevel.SpecificEnemyMove:
                    highlightRenderer.material = SpecificEnemyMaterial;
                    break;
                case HighlightLevel.GlobalEnemyMove:
                    highlightRenderer.material = GlobalEnemyMaterial;
                    break;
            }

            return highlight;
        }

    }
}