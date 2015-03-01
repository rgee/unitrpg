using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpStat : MonoBehaviour {
    private GameObject text;
    private GameObject burst;

    public void Start() {
        text = transform.FindChild("Stat Text").gameObject;
        burst = transform.FindChild("Level Up Burst").gameObject;
    }

    public void Increase() {
        Text statValue = text.GetComponent<Text>();
        int number = Int32.Parse(statValue.text);

        statValue.text = (number + 1).ToString();

        burst.GetComponent<Animator>().SetTrigger("Burst");
        text.GetComponent<Animator>().SetTrigger("Burst");
    }
}
