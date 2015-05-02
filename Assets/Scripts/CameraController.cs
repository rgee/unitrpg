using UnityEngine;

public class CameraController : MonoBehaviour {
    protected bool locked;
    public float speed = 10;

    public virtual void Lock() {
        locked = true;
    }

    public virtual void Unlock() {
        locked = false;
    }

    // Update is called once per frame
    public void Update() {
        if (!locked) {
            var horizontalSpeed = Input.GetAxis("Horizontal")*speed*Time.deltaTime;
            var veritcalSpeed = Input.GetAxis("Vertical")*speed*Time.deltaTime;
            transform.Translate(new Vector3(horizontalSpeed, veritcalSpeed, 0));
        }
    }
}