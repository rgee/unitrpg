using System;
using Contexts.MainMenu.Models;
using strange.extensions.mediation.impl;
using UnityEngine;
using UnityEngine.UI;

namespace Contexts.MainMenu.Views {
    [RequireComponent(typeof(Image))]
    public class MainMenuBackgroundView : View {
        public Sprite Sunrise;
        public Sprite Day;
        public Sprite Sunset;
        public Sprite Night;

        private Image _background;

        void Awake() {
            _background = GetComponent<Image>();
            _background.enabled = false;
        }

        public void SetTime(TimeOfDay time) {
            switch (time) {
                case TimeOfDay.Sunrise:
                    _background.sprite = Sunrise;
                    break;
                case TimeOfDay.Day:
                    _background.sprite = Day;
                    break;
                case TimeOfDay.Sunset:
                    _background.sprite = Sunset;
                    break;
                case TimeOfDay.Night:
                    _background.sprite = Night;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("time", time, null);
            }
            _background.enabled = true;
        }
    }
}