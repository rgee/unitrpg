using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoManager : Singleton<UnitInfoManager> {
    public GameObject MenuPrefab;

    private GameObject MenuInstance;
    private static string RESOURCE_PATH = "Portraits"; 

    private Text HPText;
    private Text MovText;
    private Text NameText;
    private Text LevelText;

    private List<GameObject> Portraits = new List<GameObject>();

    public void Start() {
        MenuInstance = Instantiate(MenuPrefab) as GameObject;
        HPText = FindWithinSummary("HP/Value").GetComponent<Text>();
        MovText = FindWithinSummary("Movement/Value").GetComponent<Text>();
        NameText = FindWithinSummary("Name").GetComponent<Text>();
        LevelText = FindWithinSummary("Level").GetComponent<Text>();

        // Attach the menu to the main camera
        MenuInstance.GetComponent<Canvas>().worldCamera = CombatObjects.GetCameraController().gameObject.GetComponent<Camera>();
        MenuInstance.SetActive(false);
    }

    private GameObject FindWithinMenu(String path) {
        return MenuInstance.transform.FindChild(path).gameObject;
    }

    private GameObject FindWithinSummary(String path) {
        return FindWithinMenu("Summary Panel/Summary Text/" + path);
    }
    
    public void ShowUnitInfo(Grid.Unit unit) {
        if (MenuInstance.activeSelf) {
            Debug.LogWarning("Call to activate unit info menu that is already active.");
        }

        Models.Unit model = unit.model;

        string name = model.Character.Name;
        HPText.text = String.Format("{0}/{1}", model.Health, model.Character.MaxHealth);
        MovText.text = model.Character.Movement.ToString();
        NameText.text = name.ToUpper();
        LevelText.text = "LEVEL " + model.Character.Level.ToString();

        if (unit.friendly) {
            AddPortraits(name);
        } else {
            ClearPortraits();
        }

        MenuInstance.SetActive(true);
    }

    private void ClearPortraits() {
        foreach (GameObject portrait in Portraits) {
            Destroy(portrait);
        }

        Portraits.Clear();
    }

    private void AddPortraits(String unitName) {
        GameObject bigPortrait = Resources.Load(RESOURCE_PATH + "/" + unitName) as GameObject;
        GameObject headPortrait = Resources.Load(RESOURCE_PATH + "/" + unitName + "_head") as GameObject;

        GameObject portraitContainer = FindWithinMenu("Summary Panel/Small Portrait");
        GameObject headPortraitInstance = Instantiate(headPortrait) as GameObject;
        headPortraitInstance.transform.SetParent(portraitContainer.transform);

        RectTransform rectTransform = headPortraitInstance.GetComponent<RectTransform>();
        rectTransform.offsetMax = new Vector2();
        rectTransform.offsetMin = new Vector2();
        rectTransform.localScale = new Vector3(1,1,1);
        Portraits.Add(headPortraitInstance);
    }

    public void HideUnitInfo() {
        MenuInstance.SetActive(false);
    }
}
