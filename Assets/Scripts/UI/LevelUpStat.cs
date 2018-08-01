using UnityEngine;
using UnityEngine.UI;

public class LevelUpStat : MonoBehaviour {
    private GameObject burst;
    private GameObject text;

    public void Start() {
        text = transform.Find("Stat Text").gameObject;
        burst = transform.Find("Level Up Burst").gameObject;
    }

    public void Increase() {
        var statValue = text.GetComponent<Text>();
        var number = int.Parse(statValue.text);

        statValue.text = (number + 1).ToString();

        burst.GetComponent<Animator>().SetTrigger("Burst");
        text.GetComponent<Animator>().SetTrigger("Burst");
    }
}