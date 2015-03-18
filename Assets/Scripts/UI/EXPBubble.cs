using UnityEngine;
using UnityEngine.UI;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class EXPBubble : MonoBehaviour {
	private RectTransform InnerCircleTransform;
	private GameObject InnerCircle;
	private GameObject OuterCircle;
	public float ExpPercent = 0;

	void Start () {
		InnerCircle = transform.FindChild("Inner").gameObject;
		OuterCircle = transform.FindChild("Outer").gameObject;
		InnerCircleTransform = InnerCircle.GetComponent<RectTransform>();
		StartCoroutine(FlipToEmpty());
	}
	
	// Update is called once per frame
	void Update () {
		float expMultiplier = Math.Min(ExpPercent / 100f, 100);
		float scale = 1.04f * expMultiplier;

		InnerCircleTransform.localScale = new Vector3(scale, scale, 1);
	}

	public IEnumerator FlipToEmpty() {
		GameObject dupeOuter = Instantiate(OuterCircle);
		GameObject dupeInner = Instantiate(InnerCircle);

		dupeOuter.transform.SetParent(transform);
		dupeInner.transform.SetParent(transform);
		dupeInner.transform.localScale = new Vector3();
		yield return new WaitForSeconds(3);
		yield return RotateAboutY(new RotationRequest(dupeOuter, dupeInner, 90f), new RotationRequest(InnerCircle, OuterCircle, 90f), 1f);

		Destroy(InnerCircle);
		Destroy(OuterCircle);

		InnerCircle = dupeInner;
		OuterCircle = dupeOuter;
		InnerCircleTransform = dupeInner.GetComponent<RectTransform>();
		ExpPercent = 0;

		yield return RotateAboutY(new RotationRequest(dupeOuter, dupeInner, 180f), 1f);
	}
		

	public IEnumerator AnimateToExp(float targetPercent) {
		return IncreaseExp(Math.Min(targetPercent, 100 - ExpPercent), 1f);
	}

	private struct RotationRequest {
		public GameObject Outer;
		public GameObject Inner;
		public float TargetYRotation;
		public float StartYRotation;

		public RotationRequest(GameObject outer, GameObject inner, float yRotation) {
			this.Outer = outer;
			this.Inner = inner;
			this.TargetYRotation = yRotation;
			this.StartYRotation = Outer.transform.rotation.y;
		}
	}

	IEnumerator RotateAboutY(RotationRequest front, float time) {
		float elapsedTime = 0;
		
		while (elapsedTime < time) {
			float frontRot = Mathf.Lerp(front.StartYRotation, front.TargetYRotation, (elapsedTime/time));

			front.Outer.GetComponent<RectTransform>().localRotation = new Quaternion(0, frontRot, 0, 0);
			front.Inner.GetComponent<RectTransform>().localRotation = new Quaternion(0, frontRot, 0, 0);
			
			elapsedTime += Time.deltaTime;
			yield return null;
		}	
	}

	IEnumerator RotateAboutY(RotationRequest front, RotationRequest back, float time) {
		float elapsedTime = 0;

		while (elapsedTime < time) {
			float frontRot = Mathf.Lerp(front.StartYRotation, front.TargetYRotation, (elapsedTime/time));
			float backRot = Mathf.Lerp(back.StartYRotation, back.TargetYRotation, (elapsedTime/time));

			front.Outer.GetComponent<RectTransform>().localRotation = new Quaternion(0, frontRot, 0, 0);
			front.Inner.GetComponent<RectTransform>().localRotation = new Quaternion(0, frontRot, 0, 0);

			back.Outer.GetComponent<RectTransform>().localRotation = new Quaternion(0, backRot, 0, 0);
			back.Inner.GetComponent<RectTransform>().localRotation = new Quaternion(0, backRot, 0, 0);

			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}

	IEnumerator IncreaseExp(float endingExp, float time) {
		float elapsedTime = 0;
		float startingExp = ExpPercent;

		while (elapsedTime < time) {
			ExpPercent = Mathf.Lerp (startingExp, endingExp, (elapsedTime / time));
			elapsedTime += Time.deltaTime;
			yield return null;
		}
	}
}
