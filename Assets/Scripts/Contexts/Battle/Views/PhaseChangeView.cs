using System;
using System.Collections;
using Contexts.Battle.Models;
using DG.Tweening;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Contexts.Battle.Views {
    public class PhaseChangeView : View {
        public float SlideSeconds = 0.7f;
        public float CenterPauseSeconds = 0.7f;

        private GameObject _text;

        void Awake() {
            base.Awake();
            _text = transform.FindChild("Phase Text").gameObject;
        }

        public IEnumerator ShowPhaseChangeText(BattlePhase phase) {
            Debug.Log("Phase changing to: " + phase);

            var text = _text.GetComponent<Text>();
            switch (phase) {
                case BattlePhase.Player:
                    text.text = "Player Phase";
                    break;
                case BattlePhase.Enemy:
                    text.text = "Enemy Phase";
                    break;
                case BattlePhase.Other:
                    text.text = "Other Phase";
                    break;
                default:
                    throw new ArgumentOutOfRangeException("phase", phase, null);
            }

            var textTransform = _text.GetComponent<RectTransform>();
            var thisTransform = GetComponent<RectTransform>();

            textTransform.anchorMax = new Vector2(0f, 0.5f);
            textTransform.anchorMin = new Vector2(0f, 0.5f);

            // Position the text all the way off the left hand side of the screen.
            textTransform.anchoredPosition = new Vector3(-textTransform.rect.width/2, 0, textTransform.position.z);
            var center = new Vector2(thisTransform.rect.width/2, 0);
            var offscreen = new Vector2(thisTransform.rect.width + textTransform.rect.width, 0);

            var seq = DOTween.Sequence().Append(textTransform.DOAnchorPos(center, SlideSeconds).SetEase(Ease.OutCubic)).AppendInterval(CenterPauseSeconds).Append(textTransform.DOAnchorPos(offscreen, SlideSeconds).SetEase(Ease.OutCubic));

            yield return seq.WaitForCompletion();
        }
    }
}