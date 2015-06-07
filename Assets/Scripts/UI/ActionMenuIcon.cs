using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class ActionMenuIcon : MonoBehaviour {
    public float FadeTimeSeconds = 0.2f;

    private Image _icon;
    private Text _text;

    public void Start() {
        _icon = transform.FindChild("Icon").GetComponent<Image>();
        _text = transform.FindChild("Text").GetComponent<Text>();


        var color = _text.color;
        _text.color = new Color(color.r, color.g, color.b, 0);
    }

    public void ShowText() {
        iTween.ValueTo(_icon.gameObject, iTween.Hash(
            "from", _icon.color.a,
            "to", 0,
            "time", FadeTimeSeconds,
            "onupdatetarget", gameObject,
            "onupdate", "OnIconUpdate"
        ));

        iTween.ValueTo(_text.gameObject, iTween.Hash(
            "from", _text.color.a,
            "to", 1,
            "onupdatetarget", gameObject,
            "time", FadeTimeSeconds,
            "onupdate", "OnTextUpdate"
        ));
    }

    public void HideText() {
        iTween.ValueTo(_icon.gameObject, iTween.Hash(
            "from", _icon.color.a,
            "to", 1,
            "onupdatetarget", gameObject,
            "time", FadeTimeSeconds,
            "onupdate", "OnIconUpdate"
        ));

        iTween.ValueTo(_text.gameObject, iTween.Hash(
            "from", _text.color.a,
            "to", 0,
            "onupdatetarget", gameObject,
            "time", FadeTimeSeconds,
            "onupdate", "OnTextUpdate"
        ));
    }

    public void OnIconUpdate(float alpha) {
        _icon.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, alpha);
    }

    public void OnTextUpdate(float alpha) {
        _text.color = new Color(_icon.color.r, _icon.color.g, _icon.color.b, alpha);
    }

}
