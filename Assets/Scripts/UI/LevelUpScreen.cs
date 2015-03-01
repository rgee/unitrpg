using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class LevelUpScreen : MonoBehaviour {

    public List<GameObject> Portraits;
    public Models.LevelUpResults Results;

    private Models.Character character;
    public Models.Character Character {
        set {
            
            headerText.GetComponent<Text>().text = value.Name.ToUpper() + " LEVEL " + value.level;
            UpdatePortrait(value);

            character = value;
        }
    }

    private GameObject headerText;
    private GameObject currentPortrait;
    private GameObject portraitContainer;

	// Use this for initialization
	void Start () {
        headerText = transform.FindChild("Header").gameObject;
        portraitContainer = transform.FindChild("Character").gameObject;
	}

    private void UpdatePortrait(Models.Character character) {
        Destroy(currentPortrait);
        foreach (GameObject obj in Portraits) {
            Portrait portraitScript = obj.GetComponent<Portrait>();
            if (portraitScript.CharacterName == character.Name) {
                currentPortrait = Instantiate(obj, new Vector3(), Quaternion.identity) as GameObject;
                currentPortrait.transform.SetParent(portraitContainer.transform, false);
                return;
            }
        }
    }
}
