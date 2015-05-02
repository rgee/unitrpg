using UnityEngine;
using UnityEngine.UI;

public class LevelUpStat : MonoBehaviour {
    private GameObject burst;
    private GameObject text;

    public void Start() {
        text = transform.FindChild("Stat Text").gameObject;
        burst = transform.FindChild("Level Up Burst").gameObject;
    }

    public void Increase() {
        var statValue = text.GetComponent<Text>();
        var number = int.Parse(statValue.text);

        statValue.text = (number + 1).ToString();

        burst.GetComponent<Animator>().SetTrigger("Burst");
        text.GetComponent<Animator>().SetTrigger("Burst");
    }
}