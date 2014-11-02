using UnityEngine;
using System.Collections;

public class FixedAspectRatio : MonoBehaviour {

	public float aspectRatioWidth = 16f;
	public float aspectRatioHeight = 9f;

	void Start () {
		InvokeRepeating ("Resize", 0, 0.2f);
	}

	void Resize () {
		
		float targetAspectRatio = aspectRatioWidth / aspectRatioHeight;
		float windowAspectRatio = (float)Screen.width / (float)Screen.height;
		
		float scalingFactor = windowAspectRatio / targetAspectRatio;
		
		Camera camera = GetComponent<Camera>();

		if (scalingFactor < 1.0f) {  
			Rect rect = camera.rect;
			
			rect.width = 1.0f;
			rect.height = scalingFactor;
			rect.x = 0;
			rect.y = (1.0f - scalingFactor) / 2.0f;
			
			camera.rect = rect;
		} else {
			float scalewidth = 1.0f / scalingFactor;
			
			Rect rect = camera.rect;
			
			rect.width = scalewidth;
			rect.height = 1.0f;
			rect.x = (1.0f - scalewidth) / 2.0f;
			rect.y = 0;
			
			camera.rect = rect;
		}
	}



}
