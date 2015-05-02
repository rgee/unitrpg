using System;
using System.Collections;
using UnityEngine;

public class EXPBubble : MonoBehaviour {
    public float ExpPercent;
    private GameObject InnerCircle;
    private RectTransform InnerCircleTransform;
    private GameObject OuterCircle;

    private void Start() {
        InnerCircle = transform.FindChild("Inner").gameObject;
        OuterCircle = transform.FindChild("Outer").gameObject;
        InnerCircleTransform = InnerCircle.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    private void Update() {
        var expMultiplier = Math.Min(ExpPercent/100f, 100);
        var scale = 1.04f*expMultiplier;

        InnerCircleTransform.localScale = new Vector3(scale, scale, 1);
    }

    public IEnumerator FlipToEmpty() {
        var dupeOuter = Instantiate(OuterCircle);
        var dupeInner = Instantiate(InnerCircle);

        dupeOuter.transform.SetParent(transform);
        dupeInner.transform.SetParent(transform);
        dupeInner.transform.localScale = new Vector3();
        yield return new WaitForSeconds(3);
        yield return
            RotateAboutY(new RotationRequest(dupeOuter, dupeInner, 90f),
                new RotationRequest(InnerCircle, OuterCircle, 90f), 1f);

        Destroy(InnerCircle);
        Destroy(OuterCircle);

        InnerCircle = dupeInner;
        OuterCircle = dupeOuter;
        InnerCircleTransform = dupeInner.GetComponent<RectTransform>();
        ExpPercent = 0;

        yield return RotateAboutY(new RotationRequest(dupeOuter, dupeInner, 180f), 1f);
    }

    public IEnumerator AnimateToExp(float targetPercent, float timeInSeconds) {
        yield return StartCoroutine(IncreaseExp(Math.Min(targetPercent, 100 - ExpPercent), timeInSeconds));
    }

    private IEnumerator RotateAboutY(RotationRequest front, float time) {
        float elapsedTime = 0;

        while (elapsedTime < time) {
            var frontRot = Mathf.Lerp(front.StartYRotation, front.TargetYRotation, (elapsedTime/time));

            front.Outer.GetComponent<RectTransform>().localRotation = new Quaternion(0, frontRot, 0, 0);
            front.Inner.GetComponent<RectTransform>().localRotation = new Quaternion(0, frontRot, 0, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator RotateAboutY(RotationRequest front, RotationRequest back, float time) {
        float elapsedTime = 0;

        while (elapsedTime < time) {
            var frontRot = Mathf.Lerp(front.StartYRotation, front.TargetYRotation, (elapsedTime/time));
            var backRot = Mathf.Lerp(back.StartYRotation, back.TargetYRotation, (elapsedTime/time));

            front.Outer.GetComponent<RectTransform>().localRotation = new Quaternion(0, frontRot, 0, 0);
            front.Inner.GetComponent<RectTransform>().localRotation = new Quaternion(0, frontRot, 0, 0);

            back.Outer.GetComponent<RectTransform>().localRotation = new Quaternion(0, backRot, 0, 0);
            back.Inner.GetComponent<RectTransform>().localRotation = new Quaternion(0, backRot, 0, 0);

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator IncreaseExp(float endingExp, float time) {
        float elapsedTime = 0;
        var startingExp = ExpPercent;

        while (elapsedTime < time) {
            ExpPercent = Mathf.Lerp(startingExp, endingExp, (elapsedTime/time));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        ExpPercent = endingExp;
    }

    private struct RotationRequest {
        public readonly GameObject Inner;
        public readonly GameObject Outer;
        public readonly float StartYRotation;
        public readonly float TargetYRotation;

        public RotationRequest(GameObject outer, GameObject inner, float yRotation) {
            Outer = outer;
            Inner = inner;
            TargetYRotation = yRotation;
            StartYRotation = Outer.transform.rotation.y;
        }
    }
}