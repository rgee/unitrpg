using Combat;
using UnityEngine;

public class ScreenShaker : MonoBehaviour {
    public float CritIntensity = 6;
    private Vector3 OriginalPos;
    private Quaternion OriginalRot;
    public float RegularIntensity = 4;
    public float ShakeDecay = 0.01f;
    private float ShakeIntensity;
    private bool Shaking;

    private void Start() {
        ShakeIntensity = 0;
        CombatEventBus.HitEvents.AddListener(HandleHit);
    }

    private void OnDestroy() {
        CombatEventBus.HitEvents.RemoveListener(HandleHit);
    }

    private void HandleHit(HitEvent hitEvent) {
        var hit = hitEvent.Data;
        if (hit.Missed) {
            return;
        }

        if (hit.Crit) {
            CritShake();
        } else {
            Shake();
        }
    }

    // Update is called once per frame
    private void Update() {
        if (ShakeIntensity > 0) {
            Vector3 offset = (Random.insideUnitCircle*ShakeIntensity);
            transform.position = OriginalPos + new Vector3(offset.x, offset.y, 0);
            ShakeIntensity -= ShakeDecay;
        } else if (Shaking) {
            Shaking = false;
        }
    }

    public void Shake() {
        OriginalPos = transform.position;
        OriginalRot = transform.rotation;
        Shaking = true;
        ShakeIntensity = RegularIntensity;
    }

    public void CritShake() {
        OriginalPos = transform.position;
        OriginalRot = transform.rotation;
        Shaking = true;
        ShakeIntensity = CritIntensity;
    }
}