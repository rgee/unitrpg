using DG.Tweening;
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
        _icon.DOFade(0f, FadeTimeSeconds);
        _text.DOFade(1f, FadeTimeSeconds);
    }

    public void HideText() {
        _icon.DOFade(1f, FadeTimeSeconds);
        _text.DOFade(0f, FadeTimeSeconds);
    }
}
