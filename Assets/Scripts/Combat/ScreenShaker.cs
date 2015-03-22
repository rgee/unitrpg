using UnityEngine;
using System.Collections;

public class ScreenShaker : MonoBehaviour {
	
	public bool Shaking = false;
	public float ShakeDecay = 0.01f;
	public float DefaultShakeIntensity = 0.1f;

	private float ShakeIntensity = 0;    

	private Vector3 OriginalPos;
	private Quaternion OriginalRot;

	void Start() {
		ShakeIntensity = 0;
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
		Debug.Log("Shake yo booty");
		OriginalPos = transform.position;
		OriginalRot = transform.rotation;
		Shaking = true;
		ShakeIntensity = DefaultShakeIntensity;
	}    
}