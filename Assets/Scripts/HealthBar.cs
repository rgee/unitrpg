using UnityEngine;
using System.Collections;

public class HealthBar : MonoBehaviour {

	public GameObject clipMask;
	public float healthPct = 100;

	private RectTransform maskTransform;
	private float initialX;

	void Start () {
		maskTransform = clipMask.GetComponent<RectTransform>();
		initialX = maskTransform.sizeDelta.x;
	}
	
	void Update () {
		Vector2 size = maskTransform.sizeDelta;
		maskTransform.sizeDelta = new Vector2((healthPct / 100) * initialX, size.y);
	}
}
