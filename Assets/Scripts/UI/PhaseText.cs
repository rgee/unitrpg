using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PhaseText : MonoBehaviour {

	private RectTransform RectTransform;
	private Vector2 OffScreen;
	private float MoveTime;
	private float Delay;
	private Action OnComplete;

	public void Awake() {
		RectTransform = GetComponent<RectTransform>();
	}

	public void MoveThroughScreen(Vector2 center, Vector2 offScreen, float moveTime, float delay, Action onComplete) {
		MoveTime = moveTime;
		Delay = delay;
		OnComplete = onComplete;
		OffScreen = offScreen;

		iTween.ValueTo(gameObject, iTween.Hash(
			"time", moveTime,
			"from", RectTransform.anchoredPosition,
			"to", center,
			"onupdate", "SetNewPosition",
			"oncomplete", "MoveOffScreen"
		));
	}

	private void SetNewPosition(Vector2 pos) {
		RectTransform.anchoredPosition = pos;
	}

	private void MoveOffScreen() {
		iTween.ValueTo(gameObject, iTween.Hash(
			"time", MoveTime,
			"delay", Delay,
			"from", RectTransform.anchoredPosition,
			"to", OffScreen,
			"onupdate", "SetNewPosition",
			"oncomplete", "OnMoveComplete"
		));
	}

	private void OnMoveComplete() {
		OnComplete();
	}
}
