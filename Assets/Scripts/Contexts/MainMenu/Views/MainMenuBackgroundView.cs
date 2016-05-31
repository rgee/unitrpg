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


        public void SetTime(TimeOfDay time) {
            var background = GetComponent<Image>();
            switch (time) {
                case TimeOfDay.Sunrise:
                    background.sprite = Sunrise;
                    break;
                case TimeOfDay.Day:
                    background.sprite = Day;
                    break;
                case TimeOfDay.Sunset:
                    background.sprite = Sunset;
                    break;
                case TimeOfDay.Night:
                    background.sprite = Night;
                    break;
                default:
                    throw new ArgumentOutOfRangeException("time", time, null);
            }
            background.enabled = true;
        }
    }
}