using UnityEngine;

public class Pip : MonoBehaviour {
    public GameObject Disabled;
    private GameObject DisabledInstance;
    public GameObject Enabled;
    private GameObject EnabledInstance;
    public bool IsEnabled = true;
    // Use this for initialization
    private void Start() {
        EnabledInstance = Instantiate(Enabled);
        DisabledInstance = Instantiate(Disabled);

        EnabledInstance.transform.parent = transform;
        EnabledInstance.transform.localPosition = new Vector3();

        DisabledInstance.transform.parent = transform;
        DisabledInstance.transform.localPosition = new Vector3();
    }

    // Update is called once per frame
    private void Update() {
        EnabledInstance.SetActive(IsEnabled);
        DisabledInstance.SetActive(!IsEnabled);
    }
}