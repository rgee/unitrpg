using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/**
 * This AI brain on every turn just tries to seek out a target.
 * Can hold off until the target is within a certain range, as well.
 */
[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Grid.Unit))]
public class SingleMindedFury : MonoBehaviour, AIStrategy {
    public GameObject Target;
    public int AggroRange = int.MaxValue;
    public Grid.UnitManager UnitManager;

    private Seeker Seeker;

    public void Awake() {
        Seeker = GetComponent<Seeker>();
    }


    public IEnumerator act() {
        Debug.Log("RAGE");
        yield return null;
    }
}