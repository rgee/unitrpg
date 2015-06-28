using UnityEngine;

public class CombatCursor : Singleton<CombatCursor> {
    private Animator _animator;
    private GameObject _confirmObject;
    private GameObject _defaultObject;

    private RectTransform _confirmTransform;
    private RectTransform _defaultTransform;

    public GameObject DefaultStatePrefab;
    public GameObject ConfirmStatePrefab;
    public bool Confirming { get; set; }

    public bool Interactive {
        get { return _animator.GetBool("Interactive"); }
        set { _animator.SetBool("Interactive", value); }
    }

    public void Start() {
        _animator = GetComponent<Animator>();
        _confirmObject = Instantiate(ConfirmStatePrefab);
        _defaultObject = Instantiate(DefaultStatePrefab);

        _confirmObject.transform.SetParent(transform);
        _defaultObject.transform.SetParent(transform);

        _confirmObject.SetActive(false);

        _confirmTransform = _confirmObject.GetComponent<RectTransform>();
        _defaultTransform = _defaultObject.GetComponent<RectTransform>();
    }

    public void Update() {
        _confirmObject.SetActive(Confirming);
        _defaultObject.SetActive(!Confirming);

        var currentTransform = GetCurrentTransform();
        var size = GetCurrentTransform().sizeDelta;
        size.Scale(new Vector2(.5f * currentTransform.localScale.x, .5f * currentTransform.localScale.y));

        Debug.Log(Input.mousePosition);
        currentTransform.position = Input.mousePosition + new Vector3(size.x/2, -size.y/2, 0);
    }

    private RectTransform GetCurrentTransform() {
        return _confirmObject.activeSelf ? _confirmTransform : _defaultTransform;
    }

    public void OnDisable() {
        Cursor.visible = true;
    }

    public void OnEnable() {
        Cursor.visible = false;
    }
}