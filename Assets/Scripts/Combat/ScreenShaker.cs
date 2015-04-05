using UnityEngine;
using System.Collections;

public class ScreenShaker : MonoBehaviour {
	
	public float ShakeDecay = 0.01f;
	public float CritIntensity = 6;
	public float RegularIntensity = 4;

	private bool Shaking = false;
	private float ShakeIntensity = 0;   
	private Vector3 OriginalPos;
	private Quaternion OriginalRot;

	void Start() {
		ShakeIntensity = 0;
        CombatEventBus.Hits.AddListener(HandleHit);
	}

    void OnDestroy() {
        CombatEventBus.Hits.RemoveListener(HandleHit);
    }

    void HandleHit(Hit hit) {
        if (!hit.Missed) {
            if (hit.Crit) {
                CritShake();
            } else {
                Shake();
            } 
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(ShakeIntensity > 0) {
			Vector3 offset = (Random.insideUnitCircle * ShakeIntensity);
			transform.position = OriginalPos + new Vector3(offset.x, offset.y, OriginalPos.z);
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