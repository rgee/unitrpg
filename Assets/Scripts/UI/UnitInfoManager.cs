using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoManager : Singleton<UnitInfoManager> {
    private static readonly string RESOURCE_PATH = "Portraits";
    private readonly List<GameObject> Portraits = new List<GameObject>();
    private Text HPText;
    private Text LevelText;
    private GameObject MenuInstance;
    public GameObject MenuPrefab;
    private Text MovText;
    private Text NameText;

    public void Start() {
        MenuInstance = Instantiate(MenuPrefab);
        HPText = FindWithinSummary("HP/Value").GetComponent<Text>();
        MovText = FindWithinSummary("Movement/Value").GetComponent<Text>();
        NameText = FindWithinSummary("Name").GetComponent<Text>();
        LevelText = FindWithinSummary("Level").GetComponent<Text>();

        // Attach the menu to the main camera
        MenuInstance.GetComponent<Canvas>().worldCamera =
            CombatObjects.GetCameraController().gameObject.GetComponent<Camera>();
        MenuInstance.SetActive(false);
    }

    private GameObject FindWithinMenu(string path) {
        return MenuInstance.transform.FindChild(path).gameObject;
    }

    private GameObject FindWithinSummary(string path) {
        return FindWithinMenu("Summary Panel/Summary Text/" + path);
    }

    public void ShowUnitInfo(Grid.Unit unit) {
        if (MenuInstance.activeSelf) {
            Debug.LogWarning("Call to activate unit info menu that is already active.");
        }

        var model = unit.model;

        var name = model.Character.Name;
        HPText.text = string.Format("{0}/{1}", model.Health, model.Character.MaxHealth);
        MovText.text = model.Character.Movement.ToString();
        NameText.text = name.ToUpper();
        LevelText.text = "LEVEL " + model.Character.Level;

        if (unit.friendly) {
            AddPortraits(name);
        } else {
            ClearPortraits();
        }

        MenuInstance.SetActive(true);
    }

    private void ClearPortraits() {
        foreach (var portrait in Portraits) {
            Destroy(portrait);
        }

        Portraits.Clear();
    }

    private void AddPortraits(string unitName) {
        var bigPortrait = Resources.Load(RESOURCE_PATH + "/" + unitName) as GameObject;
        var headPortrait = Resources.Load(RESOURCE_PATH + "/" + unitName + "_head") as GameObject;

        var portraitContainer = FindWithinMenu("Summary Panel/Small Portrait");
        var headPortraitInstance = Instantiate(headPortrait);
        headPortraitInstance.transform.SetParent(portraitContainer.transform);

        var rectTransform = headPortraitInstance.GetComponent<RectTransform>();
        rectTransform.offsetMax = new Vector2();
        rectTransform.offsetMin = new Vector2();
        rectTransform.localScale = new Vector3(1, 1, 1);
        Portraits.Add(headPortraitInstance);


        var bigPortraitContainer = FindWithinMenu("Portrait");
        var bigPortraitInstance = Instantiate(bigPortrait);
        var bigTransform = bigPortraitInstance.GetComponent<RectTransform>();
        bigPortraitInstance.transform.SetParent(bigPortraitContainer.transform);
        bigTransform.anchoredPosition = new Vector3();
        bigTransform.localScale = new Vector3(1, 1, 1);
        Portraits.Add(bigPortraitInstance);
    }

    public void HideUnitInfo() {
        MenuInstance.SetActive(false);
    }
}