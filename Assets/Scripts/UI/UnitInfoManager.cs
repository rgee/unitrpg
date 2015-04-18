using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoManager : Singleton<UnitInfoManager> {
    public GameObject MenuPrefab;

    private GameObject MenuInstance;

    private Text HPText;
    private Text MovText;
    private Text NameText;
    private Text LevelText;

    public void Start() {
        MenuInstance = Instantiate(MenuPrefab) as GameObject;
        HPText = FindWithinSummary("HP/Label").GetComponent<Text>();
        MovText = FindWithinSummary("Movement/Label").GetComponent<Text>();
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

        HPText.text = String.Format("{0}/{1}", model.Health, model.Character.MaxHealth);
        MovText.text = model.Character.Movement.ToString();
        NameText.text = model.Character.Name.ToUpper();
        LevelText.text = "LEVEL " + model.Character.Level.ToString();

        MenuInstance.SetActive(true);
    }

    public void HideUnitInfo() {
        MenuInstance.SetActive(false);
    }
}
