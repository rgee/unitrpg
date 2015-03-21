using UnityEngine;
using System.Collections;

public class Pip : MonoBehaviour {
	public bool isEnabled;
	public GameObject Enabled;
	public GameObject Disabled;

	private GameObject EnabledInstance;
	private GameObject DisabledInstance;

	// Use this for initialization
	void Start () {
		EnabledInstance = Instantiate(Enabled);
		DisabledInstance = Instantiate(Disabled);

		EnabledInstance.transform.parent = transform;
		DisabledInstance.transform.parent = transform;
	}
	
	// Update is called once per frame
	void Update () {
		EnabledInstance.SetActive(isEnabled);
		DisabledInstance.SetActive(!isEnabled);
	}
}
