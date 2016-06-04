using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Assets.Contexts.Chapters.Chapter2.Views {
    [RequireComponent(typeof(tk2dSprite))]
    public class HouseWindowLight : MonoBehaviour {
        private tk2dSprite _sprite;

        public void TurnOut() {
            _sprite.transform.localScale = Vector3.zero;
        }

        public Tween GetTurnOutTween() {
            return _sprite.DOScale(Vector3.zero, 0.3f);
        }

        public Tween GetTurnOnTween() {
            return _sprite.DOScale(Vector3.one, 0.3f);  
        }

        void Awake() {
            _sprite = GetComponent<tk2dSprite>();
        }
    }
}