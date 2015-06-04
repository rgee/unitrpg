using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class TimeOfDayBackground : MonoBehaviour {
    public Sprite Morning;
    public Sprite Day;
    public Sprite Evening;
    public Sprite Night;

    private Image _image;

    public void Start() {
        _image = GetComponent<Image>();

        var timeOfDay = DateTime.Now.TimeOfDay;
        var hours = timeOfDay.Hours;

        if (hours > 4 && hours <= 9) {
            _image.sprite = Morning; 
        }

        if (hours > 9 && hours <= 17) {
            _image.sprite = Day;
        }

        if (hours > 17 && hours < 21) {
            _image.sprite = Evening;
        }

        if (hours >= 21 || hours <= 4) {
            _image.sprite = Night;
        }
    }
}
