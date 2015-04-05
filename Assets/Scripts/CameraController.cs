using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public float speed = 10;
    protected bool locked;

    public virtual void Lock() {
        locked = true;
    }

    public virtual void Unlock() {
        locked = false;
    }

	// Update is called once per frame
	public void Update () {
        if (!locked) {
            float horizontalSpeed = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
            float veritcalSpeed = Input.GetAxis("Vertical") * speed * Time.deltaTime;
            transform.Translate(new Vector3(horizontalSpeed, veritcalSpeed, 0));
        }

	}
}
