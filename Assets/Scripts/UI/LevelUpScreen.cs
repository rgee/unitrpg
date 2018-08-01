using System.Collections.Generic;
using Models;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpScreen : MonoBehaviour {
    private Character character;
    private GameObject currentPortrait;
    private GameObject headerText;
    private GameObject portraitContainer;
    public List<GameObject> Portraits;
    public LevelUpResults Results;

    public Character Character {
        set {
            headerText.GetComponent<Text>().text = value.Name.ToUpper() + " LEVEL " + value.Level;
            UpdatePortrait(value);

            character = value;
        }
    }

    // Use this for initialization
    private void Start() {
        headerText = transform.Find("Header").gameObject;
        portraitContainer = transform.Find("Character").gameObject;
    }

    private void UpdatePortrait(Character character) {
        Destroy(currentPortrait);
        foreach (var obj in Portraits) {
            var portraitScript = obj.GetComponent<Portrait>();
            if (portraitScript.CharacterName == character.Name) {
                currentPortrait = Instantiate(obj, new Vector3(), Quaternion.identity) as GameObject;
                currentPortrait.transform.SetParent(portraitContainer.transform, false);
                return;
            }
        }
    }
}