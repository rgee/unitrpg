using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Combat.Props {
    public class HouseLight : MonoBehaviour, IToggleableProp {
        private tk2dSprite _sprite;

        private void Awake() {
            _sprite = GetComponent<tk2dSprite>();
        }

        public IEnumerator Enable() {
            yield return _sprite.DOScale(new Vector3(1, 1, 1), 0.7f).WaitForCompletion();
        }

        public IEnumerator Disable() {
            yield return _sprite.DOScale(new Vector3(0, 0, 1), 0.7f).WaitForCompletion();
        }
    }
}