using System;
using System.Collections;
using Contexts.BattlePrep.Models;
using DG.Tweening;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Contexts.BattlePrep.Views {
    public class BattlePrepView : View {
        public Signal<BattlePrepAction> ActionClickedSignal = new Signal<BattlePrepAction>();
        private Text _objectiveDescription;

        void Awake() {
            _objectiveDescription = transform.FindChild("Objective/Description").GetComponent<Text>();
        }

        public void SelectAction(string name) {
            var action = (BattlePrepAction) Enum.Parse(typeof (BattlePrepAction), name);
            ActionClickedSignal.Dispatch(action);
        }

        public void UpdateObjective(string description) {
            var capped = description.ToUpper();
            _objectiveDescription.text = capped;
        }

        public IEnumerator TransitionIn() {
            var objectiveContainer = transform.FindChild("Objective").GetComponent<RectTransform>();
            var buttonContainer = transform.FindChild("Buttons").GetComponent<RectTransform>();

            var seq = DOTween.Sequence()
                .Insert(0, buttonContainer.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.OutCubic))
                .Insert(0, objectiveContainer.DOAnchorPos(Vector2.zero, 0.3f).SetEase(Ease.OutCubic));
            yield return seq.WaitForCompletion();
        }

        public IEnumerator TransitionOut() {
            var objectiveContainer = transform.FindChild("Objective").GetComponent<RectTransform>();
            var buttonContainer = transform.FindChild("Buttons").GetComponent<RectTransform>();

            var seq = DOTween.Sequence()
                .Insert(0, buttonContainer.DOAnchorPos(new Vector2(0, -100), 0.3f).SetEase(Ease.OutCubic))
                .Insert(0, objectiveContainer.DOAnchorPos(new Vector2(0, 100), 0.3f).SetEase(Ease.OutCubic));
            yield return seq.WaitForCompletion();
        }
    }
}