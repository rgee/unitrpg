using UnityEngine;
using System.Collections;

public class Pip : MonoBehaviour {
	public bool IsEnabled = true;
	public GameObject Enabled;
	public GameObject Disabled;

	private GameObject EnabledInstance;
	private GameObject DisabledInstance;

	// Use this for initialization
	void Start () {
		EnabledInstance = Instantiate(Enabled);
		DisabledInstance = Instantiate(Disabled);

		EnabledInstance.transform.parent = transform;
		EnabledInstance.transform.localPosition = new Vector3();

		DisabledInstance.transform.parent = transform;
		DisabledInstance.transform.localPosition = new Vector3();
	}
	
	// Update is called once per frame
	void Update () {
		EnabledInstance.SetActive(IsEnabled);
		DisabledInstance.SetActive(!IsEnabled);
	}
}
